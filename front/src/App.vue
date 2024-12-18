<script lang="ts" setup>
import { ref, onMounted } from 'vue';
import cepInput from './components/cep-input.vue';

interface registro {
    id: number
    de: string
    para: string
    distancia: number
}

const cepModal1 = ref('');
const cepModal2 = ref('');
const cepTable1 = ref('');
const cepTable2 = ref('');
const username = ref('');
const usernameRegister = ref('');
const emailRegister = ref('');
const passwordRegister = ref('');
const cPasswordRegister = ref('');
const emailLogin = ref('');
const passwordLogin = ref('');
const registros = ref<registro[]>([]);
const carregando = ref(true);
const importando = ref(false);
const registrando = ref(false);
const entrando = ref(false);
const showPopup = ref(false);
const calculando = ref(false);
const popupMessage = ref('');
const typePopup = ref('bg-success');
const msImportando = ref('importar');
const msCalcular = ref('Calcular');
const msRegistrando = ref('Registrar');
const msEntrando = ref('Entrar');

function errorPopUp(message: string) {
    popupMessage.value = message;
    typePopup.value = 'bg-danger';
    showPopup.value = true;
    setTimeout(() => (showPopup.value = false), 10000);
}

function successPopup(message: string) {
    popupMessage.value = message;
    typePopup.value = 'bg-success';
    showPopup.value = true;
    setTimeout(() => (showPopup.value = false), 10000);
}

async function fetchRegistros() {
    carregando.value = true;
    let filter = "";
    if (cepTable1.value && cepTable2.value) {
        filter = `?de=${cepTable1.value}&para=${cepTable2.value}`;
    } else if (cepTable1.value) {
        filter = `?de=${cepTable1.value}`;
    } else if (cepTable2.value) {
        filter = `?para=${cepTable2.value}`;
    }
    try {
        const response = await fetch('http://localhost:7000/api/Distance/list' + filter, { credentials: 'include' })

        const data = await response.json();
        if (!response.ok) {
            errorPopUp(data.message);
            return
        }
        registros.value = data;
    } catch (error) {
        errorPopUp("Erro ao buscar cálculos");
    } finally {
        carregando.value = false;
    }
}

async function includeCalculo() {
    msCalcular.value = 'Calcular...';
    calculando.value = true;
    debugger

    try {
        const response = await fetch('http://localhost:7000/api/Distance/Calculate', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            credentials: 'include',
            body: JSON.stringify({
                de: cepModal1.value,
                para: cepModal2.value,
            }),
        });

        const data = await response.json();
        if (data.message) {
            errorPopUp(data.message);
        } else {
            successPopup(`Distância é de ${data.distancia}Km`);
            cepTable1.value = '';
            cepTable2.value = '';
            fetchRegistros();
        }
    } catch (message) {
        errorPopUp('Erro ao efetuar cálculo');
    } finally {
        calculando.value = false;
        msCalcular.value = 'Calcular';
    }
}

async function importarCeps(event: Event) {
    const arquivo = (event.target as HTMLInputElement).files?.[0];
    if (!arquivo) return;

    const formData = new FormData();
    formData.append('arquivo', arquivo);
    formData.append('funcao', 'importarCeps');
    importando.value = true;
    msImportando.value = 'Importando...';

    try {
        const response = await fetch('http://localhost:7000/api/importar', {
            method: 'POST',
            credentials: 'include',
            body: formData,
        });

        const data = await response.json();
        if (data.message) {
            errorPopUp(data.message);
        } else {
            successPopup(data.success);
            fetchRegistros();
        }
    } catch (error) {
        errorPopUp('Erro ao importar CEPs');
    } finally {
        importando.value = false;
        msImportando.value = 'Importar';
    }
}

async function includeUser() {
    if (passwordRegister.value != cPasswordRegister.value) {
        errorPopUp('Senhas não conferem')
        return
    }
    msRegistrando.value = 'Registrando...';
    registrando.value = true;

    try {
        const response = await fetch('http://localhost:7000/api/User/register', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            credentials: 'include',
            body: JSON.stringify({
                username: usernameRegister.value,
                email: emailRegister.value,
                password: passwordRegister.value,
            }),
        });

        const data = await response.json();
        if (data.message) {
            errorPopUp(data.message);
        } else {
            successPopup("Registro efetuado com sucesso.");
        }
    } catch (error) {
        errorPopUp('Erro ao se registrar');
    } finally {
        registrando.value = false;
        msRegistrando.value = 'Registrar';
    }
}

async function loginUser() {
    msEntrando.value = 'Entrando...';
    entrando.value = true;

    try {
        const response = await fetch('http://localhost:7000/api/User/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            credentials: 'include',
            body: JSON.stringify({
                email: emailLogin.value,
                password: passwordLogin.value,
            }),
        });

        const data = await response.json();
        if (data.message) {
            errorPopUp(data.message);
            msEntrando.value = 'Entrar';
            entrando.value = false;
        } else {
            username.value = data.username
            successPopup("Login efetuado com sucesso.");
            msEntrando.value = 'Já conectado';
            fetchRegistros()
        }
    } catch (error) {
        errorPopUp('Erro ao efetuar login');
        msEntrando.value = 'Entrar';
        entrando.value = false;
    }
}

async function logout() {
    username.value = ""
    msEntrando.value = 'Entrar';
    entrando.value = false;
    registros.value = []
    successPopup("Desconectado.");
    await fetch('http://localhost:7000/api/User/logout', { credentials: 'include' });
}

async function verifyLogin() {
    const response = await fetch('http://localhost:7000/api/User/', {
        credentials: 'include'
    });
    const data = await response.json();
    username.value = data.username

}

onMounted(() => {
    verifyLogin()
    fetchRegistros()
});
</script>

<template>
    <div class="container" id="app">
        <header class="bg-dark text-white d-flex justify-content-around align-items-center p-2">
            <h1 class="h4 d-none d-md-block"><i class="bi bi-geo-alt"></i>-----DISTÂNCIA ENTRE CEPS-----<i
                    class="bi bi-geo-alt"></i></h1>
            <h1 class="h6 mt-1 d-md-none mu-1"><i class="bi bi-geo-alt"></i>--DISTÂNCIA ENTRE CEPS--<i
                    class="bi bi-geo-alt"></i>
            </h1>
            <button class="navbar-toggler text-light d-md-none" type="button" data-toggle="collapse"
                data-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
                <div class="p-2 border rounded">
                    <i class="bi bi-justify"></i>
                </div>
            </button>
        </header>
        <nav class="navbar navbar-expand-md navbar-light bg-light my-0 rounded-bottom">

            <div class="collapse navbar-collapse m-0 p-0" id="navbarNav">
                <div class="navbar-nav row w-100 m-0 p-0">
                    <button type="button" class="col text-dark bg-light d-flex justify-content-center border"
                        data-toggle="modal" data-target="#calculoModal" data-whatever="@mdo" :disabled="importando">
                        <div class="p-2">
                            <i class="bi bi-plus-circle"></i>
                            <span class="ml-2">Novo cálculo</span>
                        </div>
                    </button>
                    <!-- <input type="file" class="custom-file-input d-none" id="inputGroupFile04" accept=".csv, text/csv"
                        aria-describedby="inputGroupFileAddon04" @change="importarCeps" :disabled="importando">
                    <label class="col text-dark d-flex justify-content-center border m-0"
                        :class="{ 'loading-button': importando }" for="inputGroupFile04">
                        <div class="p-2">
                            <i class="bi bi-file-earmark-arrow-up"></i>
                            <span class="ml-2">{{ msImportando }}</span>
                        </div>
                    </label> -->
                    <button v-if="!username" type="button"
                        class="col text-dark bg-light d-flex justify-content-center border" data-toggle="modal"
                        data-target="#loginModal">
                        <div class="p-2">
                            <i class="bi bi-door-open"></i>
                            <span class="ml-2">Entrar</span>
                        </div>
                    </button>
                    <button v-if="!username" type="button"
                        class="col text-dark bg-light d-flex justify-content-center border" data-toggle="modal"
                        data-target="#registerModal">
                        <div class="p-2">
                            <i class="bi bi-person-plus"></i>
                            <span class="ml-2">Registrar</span>
                        </div>
                    </button>
                    <button v-if="username" type="button"
                        class="col text-dark bg-light d-flex justify-content-center border" disable>
                        <div class="p-2">
                            <i class="bi bi-person-circle"></i>
                            <span class="ml-2">{{ username }}</span>
                        </div>
                    </button>
                    <button v-if="username" type="button" @click="logout"
                        class="col text-dark bg-light d-flex justify-content-center border">
                        <div class="p-2">
                            <i class="bi bi-x-circle"></i>
                            <span class="ml-2">Sair</span>
                        </div>
                    </button>
                    <button type="button" class="col-1 text-dark bg-light d-flex justify-content-center border"
                        @click="fetchRegistros">
                        <div class="p-2">
                            <i class="bi bi-arrow-clockwise"></i>
                        </div>
                    </button>
                </div>
            </div>
        </nav>
        <main class="p-5">
            <div class="d-flex justify-content-center">
                <p class="text-center h2 mb-4">Lista de Ceps já calculados por você</p>
            </div>
            <form @submit.prevent="fetchRegistros">
                <div class="table">
                    <div class="input-group mb-3">
                        <cep-input v-model="cepTable1"></cep-input>
                        <cep-input v-model="cepTable2"></cep-input>
                        <button class="btn btn-outline-secondary" type="submit" id="button-addon"><i
                                class="bi bi-funnel"></i></button>
                    </div>
                </div>
            </form>
            <table class="table table-light table-striped">

                <thead class="thead-dark">
                    <tr>
                        <th scope="col">CEP 1</th>
                        <th scope="col">CEP 2</th>
                        <th scope="col">DISTÂNCIA (Km)</th>
                    </tr>
                </thead>
                <tbody v-if="carregando">
                    <tr>
                        <td colspan="3">Carregando...</td>
                    </tr>
                </tbody>
                <tbody v-else-if="registros.length === 0">
                    <tr>
                        <td colspan="3">Nenhum resultado encontrado</td>
                    </tr>
                </tbody>
                <tbody v-else>
                    <tr v-for="registro in registros" :key="registro.id">
                        <td>{{ registro.de }}</td>
                        <td>{{ registro.para }}</td>
                        <td>{{ registro.distancia }}</td>
                    </tr>
                </tbody>
            </table>

        </main>

        <a href="https://github.com/ciprianoLucas">
            <footer class="bg-dark text-white text-center p-2 fixed-bottom"> Lucas H. Cipriano &copy; 2024</footer>
        </a>

        <div class="modal fade" id="calculoModal" tabindex="-1" aria-labelledby="calculoModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="calculoModalLabel">Calcular distância</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <form @submit.prevent="includeCalculo">
                        <div class="modal-body">
                            <div class="form-group">
                                <label for="cep-1" class="col-form-label">CEP 1:</label>
                                <cep-input v-model="cepModal1"></cep-input>
                            </div>
                            <div class="form-group">
                                <label for="cep-2" class="col-form-label">CEP 2:</label>
                                <cep-input v-model="cepModal2"></cep-input>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="submit" class="btn btn-secondary" :class="{ 'loading-button': calculando }"
                                :disabled="calculando">{{ msCalcular }}</button>
                        </div>
                    </form>
                </div>
            </div>
        </div>
        <div class="modal fade" id="registerModal" tabindex="-1" aria-labelledby="registerModalLabel"
            aria-hidden="true">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="registerModalLabel">Registrar</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <form @submit.prevent="includeUser">
                        <div class="modal-body">
                            <div class="form-group">
                                <label for="username-register">Nome de usuário:</label>
                                <input id="username-register" class="form-control" v-model="usernameRegister" />
                            </div>
                            <div class="form-group">
                                <label for="email-register">E-mail:</label><br>
                                <input type="email" id="email-register" class="form-control" v-model="emailRegister" />
                            </div>
                            <div class="form-group">
                                <label for="password-register">Senha:</label><br>
                                <input type="password" id="password-register" class="form-control"
                                    v-model="passwordRegister" />
                            </div>
                            <p style="font-size: 0.8em;">A senha deve ter no mínimo 8 caracteres, letras maiúsculas,
                                minúsculas e números</p>
                            <div class="form-group">
                                <label for="c-password-register">Repita a senha:</label>
                                <input type="password" id="c-password-register" class="form-control"
                                    v-model="cPasswordRegister" />
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="submit" class="btn btn-secondary" :class="{ 'loading-button': registrando }"
                                :disabled="registrando">{{ msRegistrando }}</button>
                        </div>
                    </form>
                </div>
            </div>
        </div>
        <div class="modal fade" id="loginModal" tabindex="-1" aria-labelledby="loginModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="loginModalLabel">Entrar</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <form @submit.prevent="loginUser">
                        <div class="modal-body">
                            <div class="form-group">
                                <label for="email-login">E-mail:</label><br>
                                <input type="email" id="email-login" class="form-control" v-model="emailLogin" />
                            </div>
                            <div class="form-group">
                                <label for="cPassword-login">Senha:</label>
                                <input type="password" id="password-login" class="form-control"
                                    v-model="passwordLogin" />
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="submit" class="btn btn-secondary" :class="{ 'loading-button': entrando }"
                                :disabled="entrando">{{ msEntrando }}</button>
                        </div>
                    </form>
                </div>
            </div>
        </div>
        <transition name="fade">
            <div v-if="showPopup" class="popup" :class="typePopup">
                {{ popupMessage }}
            </div>
        </transition>
    </div>
</template>

<style scoped>
@import url("https://cdn.jsdelivr.net/npm/bootstrap@4.6.0/dist/css/bootstrap.min.css");
@import url("https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css");


body {
    font-family: "Montserrat", sans-serif;
    background-color: rgb(187, 187, 187);
}

.container {
    display: flex;
    flex-direction: column;
    min-height: 100vh;
    background-color: rgb(231, 231, 231);
    padding: 0;
}

main {
    flex: 1;
    padding: 20px;
    overflow-y: auto;
    max-height: calc(100vh - 120px);
}

nav a:hover,
nav button:hover,
nav label:hover {
    text-decoration: none;
    background: #cfcfcf;
    cursor: pointer;
}

footer {
    font-size: 0.8rem;
}

.table {
    font-size: 0.8rem;
}

/* Para navegadores WebKit (Chrome, Safari, etc.) */
::-webkit-scrollbar {
    width: 10px;
}

::-webkit-scrollbar-track {
    background: #f1f1f1;
}

::-webkit-scrollbar-thumb {
    background: #888;
    border-radius: 5px;
}

::-webkit-scrollbar-thumb:hover {
    background: #555;
}

.popup {
    position: fixed;
    bottom: 40px;
    right: 40px;
    color: #fff;
    padding: 20px 30px;
    border-radius: 5px;
    z-index: 9999;
    /* display: none; */
    animation: fadeOut 10s forwards;
}

@keyframes fadeOut {
    0% {
        opacity: 1;
    }

    90% {
        opacity: 1;
    }

    100% {
        opacity: 0;
        display: none;
    }
}

.fade-enter-active,
.fade-leave-active {
    transition: opacity 0.5s;
}

.fade-enter,
.fade-leave-to {
    opacity: 0;
}

.loading-button {
    animation: animacao 1s infinite alternate;
    cursor: grab;
}

@keyframes animacao {
    from {
        transform: translateY(-5);
    }

    to {
        transform: translateY(-5px);
    }
}
</style>

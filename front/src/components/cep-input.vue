<template>
    <input type="text" class="form-control" :class="{ 'border-danger': erro }" v-model="cep" @blur="validarCep"
        placeholder="00000-000" aria-label="CEP">
</template>

<script lang="ts" setup>
import { ref, watch } from 'vue';

const props = defineProps<{
    modelValue: string;
}>();

const emit = defineEmits<{
    (event: 'update:modelValue', value: string): void;
}>();

const cep = ref(props.modelValue);
const erro = ref(false);

function formatarCep() {
    let cepFormatado = cep.value.replace(/\D/g, '');
    if (cepFormatado.length > 8) {
        cepFormatado = cepFormatado.slice(0, 8);
    }
    cepFormatado = cepFormatado.replace(/^(\d{5})(\d{1,3})$/, '$1-$2');
    cep.value = cepFormatado;
}

function validarCep() {
    if (cep.value.length === 0 || cep.value.length === 9) {
        erro.value = false;
    } else {
        erro.value = true;
    }
}

watch(cep, (newValue) => {
    formatarCep();
    validarCep();
    emit('update:modelValue', newValue);
}, { immediate: true });
</script>

<style scoped>
.popup {
    position: fixed;
    bottom: 1rem;
    right: 1rem;
    padding: 1rem;
}

.v-enter-active {
    transition: all 0.3s ease;
}

.v-leave-active {
    transition: all 1s ease;
}

.v-enter-from {
    opacity: 0;
    transform: translateY(100px);
}

.v-leave-to {
    opacity: 0;
    transform: translateY(10px);
}
</style>

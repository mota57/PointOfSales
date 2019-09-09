
<template>
  <div>
    <img :src="ImagePicture" :width="width || 200" :height="height || 200"  data-foo="1" /><br />
    <input type="button" @click="removeImage" value="Remove" :disabled="!Image">
    <input type="file" @change="onFileChange" placeholder="Enter image" @input="$emit('input', $event.target.files[0])">
    <slot></slot>
  </div>
   
</template>

<script>
  export default {
    name:'cmp-image',
    props:['value', 'single', 'width', 'height'],
    created() {
      this.ImagePicture = 'data:text/plain;base64, '+ this.value;

    },
    data() {
      return {
        Image: '',
        ImagePicture: '',
      }
    },
    methods: {
      onFileChange(e) {
        var files = e.target.files || e.dataTransfer.files;
        if (!files.length)
          return;

        this.Image = files[0];
        this.createImageBase64(files[0]);
      },
      createImageBase64(file) {
        var reader = new FileReader();
        var vm = this;

        reader.onload = (e) => {
          vm.ImagePicture = e.target.result;
        };

        reader.readAsDataURL(file);
      },
      removeImage: function (e) {
        this.ImagePicture = '';
        this.Image = null;
      },
    }
  }
</script>

<style>
</style>


<template>
  <div>
    <img :src="ImagePicture" :width="width || 200" :height="height || 200"  /><br />
    <input type="button" @click="removeImage" value="Remove" >
    <input :name="name" type="file" @change="onFileChange" placeholder="Enter image" @input="$emit('input', $event.target.files[0])">
    <slot></slot>
  </div>
   
</template>

<script>
  export default {
    name:'cmp-image',
    props: ['value', 'name', 'imagebytes', 'single', 'width', 'height'],
    created() {
      // set the value from props.value to display the image
      this.ImagePicture = 'data:image/png;base64, ' + this.imagebytes;

    },
    data() {
      return {
        ImagePicture: '',
      }
    },
    methods: {
      isImageDeleted() {
        return this.ImagePicture ===  '' 
      },
      onFileChange(e) {
        var files = e.target.files || e.dataTransfer.files;
        if (!files.length)
          return;

        for (let i = 0; i < files.length; i++) {
          this.createImageBase64(files[i]);
        }
      },
      createImageBase64(file) {
        var reader = new FileReader();
        var vm = this;

        reader.onload = (e) => {
          console.log(e.target.result)
          vm.ImagePicture = e.target.result;
        };

        reader.readAsDataURL(file);
      },
      removeImage: function (e) {
        this.ImagePicture = ''
        console.log('removeImage');
      },
    }
  }
</script>

<style>
</style>

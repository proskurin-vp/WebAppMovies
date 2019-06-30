function setImage() {
    let fileInput = document.querySelector('#ImageUpload');
        fileInput.addEventListener('change', function () {
            var curFiles = fileInput.files;
        var posterImage = document.querySelector('#poster-image');
        posterImage.src = window.URL.createObjectURL(curFiles[0]);
        var labelImageFile = document.querySelector('#label-image-file');
        labelImageFile.innerHTML = curFiles[0].name;
        });
}

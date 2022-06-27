    $(document).ready(function() {
        var $modal = $('#img-modal');
        var image = document.getElementById('sample-img');
        var cropper;
        $('#img_holder').change(function(event) {
            var files = event.target.files;
            var done = function(url) {
                image.src = url;
                $modal.modal('show');
            };
            if (files && files.length > 0) {
                var reader = new FileReader();
                reader.onload = function(event) {
                    done(reader.result);
                };
                reader.readAsDataURL(files[0]);
            }
        });
        $modal.on('shown.bs.modal', function() {
            cropper = new Cropper(image, {
                aspectRatio: 1.0,
                viewMode: 3,
                dragCrop: true,
                movable: true,
                preview: '.crop-preview'
            });
        }).on('hidden.bs.modal', function() {
            cropper.destroy();
            cropper = null;
        });
        $('#crop').click(function() {
            var canvas = cropper.getCroppedCanvas({
                width: 800,
                height: 600,
            });
            canvas.toBlob(function(blob) {
                var url = URL.createObjectURL(blob);
                var reader = new FileReader();
                reader.readAsDataURL(blob);
                reader.onloadend = function() {
                    var base64data = reader.result;
                    document.querySelector('#b64-img').value = base64data;
                    document.querySelector('.content').innerHTML = '';
                    document.querySelector('.content').innerHTML = `<img src="${base64data}" alt="product-img">`;
                    $modal.modal('hide');
                };
            });
        });
        $('#close-modal').click(function() {
            document.querySelector('#img_holder').value = '';
        });
    });
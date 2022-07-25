$(document).ready(function () {



    $("#btn-comment").click(function () {
        
        let productId = $("#productId").val();
        let contentform = $("#contentform").val();




        axios.post('/shop/addcomment',{ productId: productId, content: content } )
        .then(function (response) {
            console.log(response);
        })
        .catch(function (error) {
            console.log(error);
        });

    });

    





});
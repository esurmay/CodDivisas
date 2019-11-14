 
$(document).ready(function () {

    function abc() {
        $('#cotizaciones').html('');
        $('#cotizacionesHora').html('');
           $.getJSON("http://localhost:64917/api/cotizaciones",
            function (data) {
                $.each(data,
                    function (key, val) {
                        var str = val.Moneda + ': ' + val.Precio;
                          $('<li>', { html: str }).appendTo($('#cotizaciones'));
                    }
                );

                var now = new Date();
                $('#cotizacionesHora').append('Ultima actualización: ' + now);


            }
        );

    }
    setInterval(abc, 5000);
});
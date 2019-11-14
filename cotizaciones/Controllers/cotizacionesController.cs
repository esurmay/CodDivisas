using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace cotizaciones.Controllers
{
    [RoutePrefix("api/cotizaciones")]
    public class cotizacionesController : ApiController
    {

        List<string> Monedas = new List<string>(new[]
       {
                 "USD"
                ,"EUR"
                ,"ARS"
            });

        //[HttpGet]
        [Route("{moneda}")]
        public ObjectResponse Get(string moneda)
        {

            ObjectResponseOrigen ValorRespuesta = new ObjectResponseOrigen();

            switch (moneda.ToLower())
            {
                case "dolar":
                    ValorRespuesta = GetValores(Monedas[0]);
                    break;

                case "euro":
                    ValorRespuesta = GetValores(Monedas[1]);
                    break;
                case "real":
                    ValorRespuesta = GetValores(Monedas[2]);
                    break;
            }

            ObjectResponse respuesta = new ObjectResponse();
            respuesta.Moneda = ValorRespuesta.result.source;
            respuesta.Precio = ValorRespuesta.result.value;

            return respuesta;
        }

        [Route("")]
        //[HttpGet]
        public List<ObjectResponse> GetAll()
        {
            List<ObjectResponseOrigen> valores = new List<ObjectResponseOrigen>();
            List<ObjectResponse> respuesta = new List<ObjectResponse>();

            //cargando todas las cotizaciones
            valores.Add(CotizacionDolar());
            valores.Add(CotizacionEuro());
            valores.Add(CotizacionReal());

            foreach (var item in valores)
            {
                //creando y agregando todas la cotizaciones al objeto: respuesta
                respuesta.Add(new ObjectResponse
                {
                    Moneda = item.result.source,
                    Precio = item.result.value,
                });
            }

            //devolviendo la lista de cotizaciones: objeto respuesta de tipo List<>
            return respuesta;
        }

        #region metodos privados

        private ObjectResponseOrigen CotizacionDolar()
        {
            return GetValores(Monedas[0]);

        }

        private ObjectResponseOrigen CotizacionEuro()
        {
            return GetValores(Monedas[1]);

        }
        private ObjectResponseOrigen CotizacionReal()
        {
            return GetValores(Monedas[2]);

        }


        private ObjectResponseOrigen GetValores(string moneda)
        {

            string url = $"https://api.cambio.today/v1/quotes/{moneda}/ARS/json?quantity=1&key=2589|rYe2kgZjCqzm0ncqHrFmNymy3eLw1ZoR";
            //objecto cliente para peticiones de apis
            System.Net.Http.HttpClient http = new System.Net.Http.HttpClient();
            //llamada y respuesta de la api
            var response = http.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
            //conversion de respuesta de la api al objeto respuesta
            var data = JsonConvert.DeserializeObject<ObjectResponseOrigen>(response);

            return data;
        }

        #endregion
    }

    /// <summary>
    /// objeto respuesta
    /// </summary>
    public class ObjectResponse
    {
        public string Moneda { get; set; }
        public string Precio { get; set; }
    }

    /// <summary>
    /// objeto respuesta origen que es la estructura que devuelve la api
    /// </summary>
    public class ObjectResponseOrigen
    {
        public Result result { get; set; }
        public string status { get; set; }
    }

    /// <summary>
    /// objeto result. propuedad ObjectResponseOrigen.result
    /// </summary>
    public class Result
    {
        public string updated { get; set; }
        public string source { get; set; }
        public string target { get; set; }
        public string value { get; set; }
        public string quantity { get; set; }
        public string amount { get; set; }
    }
}

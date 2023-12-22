using AAM_Lab16_Dominguez.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AAM_Lab16_Dominguez
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private const string BaseUrl = "https://api-airbnb-basic.onrender.com";


        public async Task<T> GetAsync<T>(string endpoint)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);

                // Realizar la solicitud GET al servidor
                HttpResponseMessage response = await client.GetAsync(endpoint);

                // Verificar si la solicitud fue exitosa (código de estado 200)
                if (response.IsSuccessStatusCode)
                {
                    // Leer y deserializar la respuesta JSON
                    string json = await response.Content.ReadAsStringAsync();
                    T result = JsonConvert.DeserializeObject<T>(json);
                    return result;
                }
                else
                {
                    // Manejar errores
                    throw new Exception($"Error en la solicitud: {response.StatusCode}");
                }
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                var result = await GetAsync<ResponseBase>("/airbnb");
                // Procesar el resultado
                var places = result.places.Select(x => new
                {
                    x.name,
                    location = x.location.country + ", " + x.location.city,
                    x.image_url

                }).ToList();

                listViewDemo.ItemsSource = places;
            }
            catch (Exception ex)
            {
                throw ex;
                // Manejar errores
            }
        }
    }
}
using System;
using System.Collections;
using System.Runtime.Serialization;
using RestSharp;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Weather
{
    public static class OpenWeather
    {
        public static string Now()
        {
            var res = "";
            try
            {
                var client = new RestClient("https://api.openweathermap.org");
                var request = new RestRequest("/data/2.5/weather?q=tomsk&appid=e0af923e12e665f3a0ca9a35f31396ca&units=metric&&lang=ru", Method.GET);
                var queryResult = client.Execute(request);
                var response =  JObject.Parse(queryResult.Content);
                var description = response["weather"][0]["description"].ToString();
                res += $"{description}\r\n";
                var temp = response["main"]["temp"].ToString();
                res += $"Температура : {temp}С\r\n";
                var wind = response["wind"]["speed"].ToString();
                res += $"Ветер : {wind} м/с\r\n";
            }
            catch (Exception ex)
            {                
                res = $"Ошибка : {ex.Message}";
            }
            
            return res;
        }
        public static string Hourly()
        {
            var res = "";
            try
            {
                var client = new RestClient("https://api.openweathermap.org");
                var request = new RestRequest("/data/2.5/onecall?lat=56.4977100&lon=84.9743700&exclude=hourly&appid=e0af923e12e665f3a0ca9a35f31396ca&units=metric&&lang=ru", Method.GET);
                var queryResult = client.Execute(request);
                var response =  JObject.Parse(queryResult.Content);
                var daily = response["daily"].ToString();
                
            }
            catch (Exception ex)
            {                
                res = $"Ошибка : {ex.Message}";
            }
            
            return res;
        }
    }
}
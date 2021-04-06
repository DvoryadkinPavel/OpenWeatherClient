using System;
using System.Collections;
using RestSharp;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Weather
{    
    public static class OpenWeather
    {
        private static string _apikey;
        private static string ApiKey 
        {
            get
            {
                if(String.IsNullOrEmpty(_apikey)) 
                {
                    _apikey = System.IO.File.ReadAllText("apikey.txt");
                }
                return _apikey;
            }
        }
        public static string Now()
        {
            var res = "";
            try
            {
                var client = new RestClient("https://api.openweathermap.org");
                var request = new RestRequest($"/data/2.5/weather?q=tomsk&appid={ApiKey}&units=metric&&lang=ru", Method.GET);
                var queryResult = client.Execute(request);
                var response =  JObject.Parse(queryResult.Content);
                var description = response["weather"][0]["description"].ToString();
                res += "Сейчас ";
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
                var request = new RestRequest($"/data/2.5/onecall?lat=56.4977100&lon=84.9743700&exclude=hourly&appid={ApiKey}&units=metric&&lang=ru", 
                                                Method.GET);
                var queryResult = client.Execute(request);
                var response =  JObject.Parse(queryResult.Content);
                res += HourlySingleDateTimeFromJObject(response, 0);
                res += HourlySingleDateTimeFromJObject(response, 1);
                res += HourlySingleDateTimeFromJObject(response, 2);
            }
            catch (Exception ex)
            {                
                res = $"Ошибка : {ex.Message}";
            }
            
            return res;
        }
        private static string HourlySingleDateTimeFromJObject(JObject response, int index)
        {
            var res = "";
            var daily = (double)response["daily"][index]["dt"];
            var DT = DateTimeConverter.UnixTimeStampToDateTime(daily);
            if(index != 0)
            {
                res += $"{DT.ToString("dd MMMM")}\r\n";
            }
            else
            {
                res += $"{DT.ToString("dd MMMM")} (сегодня)\r\n";
            }
            var description = response["daily"][index]["weather"][0]["description"].ToString();
            res += $"{description}\r\n";
            var temp = response["daily"][index]["temp"]["morn"].ToString();
            res += $"Утром : {temp}С\r\n";
            temp = response["daily"][index]["temp"]["day"].ToString();
            res += $"Днём : {temp}С\r\n";
            temp = response["daily"][index]["temp"]["eve"].ToString();
            res += $"Вечером : {temp}С\r\n";
            var wind = response["daily"][index]["wind_speed"].ToString();
            res += $"Ветер : {wind} м/с\r\n";
            res += "\r\n";
            return res;
        }
    }
}
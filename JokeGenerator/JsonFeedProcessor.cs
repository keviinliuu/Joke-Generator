﻿using System.Collections.Generic;
using Newtonsoft.Json;


namespace JokeGenerator
{
    public class JsonFeedProcessor
    {
        private readonly IJsonFeedSource _category;
        private readonly IJsonFeedSource _joke;
        private readonly IJsonFeedSource _name;

        public JsonFeedProcessor(IJsonFeedSource category, IJsonFeedSource joke, IJsonFeedSource name)
        {
            _category = category;
            _joke = joke;
            _name = name;
        }

        public List<string> GetCategories()
        {
            return new List<string>(new[]{_category.GetJsonString()});
        }

        public dynamic GetNames()
        {
            return JsonConvert.DeserializeObject<dynamic>(_name.GetJsonString());
        }

        public List<string> GetRandomJokes(string firstName, string lastName, string category)
        {
            _joke.SetOption(category);
            var joke = _joke.GetJsonString();
            joke = ReplaceName(joke, firstName, lastName);
            return new List<string>(new string[] {JsonConvert.DeserializeObject<dynamic>(joke)?.value});
        }
        
        private static string ReplaceName(string joke, string firstName, string lastName)
        {
            if (firstName == null || lastName == null) 
                return joke;

            var chuckLength = "Chuck Norris".Length;

            for (var i = 0; i <= joke.Length - chuckLength; i++)
            {
                var testString = joke.Substring(i, chuckLength);
                if (testString.Equals("Chuck Norris"))
                {
                    var firstPart = joke.Substring(0, i);
                    var secondPart = joke.Substring(i + chuckLength, joke.Length - (i + chuckLength));
                    joke = firstPart + firstName + " " + lastName + secondPart;
                }
            }

            return joke;
        }
    }
}
using System.Text.Json;
using MovieApi.Models;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace MovieApi.Helpers
{
    public class ApiHelpers
    {
        public List<MovieList> GetMoviesFromProvider(string rawJsonString) 
        {
            List<MovieList> movieList = new List<MovieList>();
            dynamic data = JsonConvert.DeserializeObject(rawJsonString);

            var moviesArray = data.Movies;

            foreach (var movie in moviesArray)
            {
                    movieList.Add(new MovieList
                    {
                        Title = movie.Title,
                        Year = movie.Year,
                        ID = movie.ID,
                        Type = movie.Type,
                        Poster = movie.Poster
                    });
            }

            return movieList;
        }

        public int LevenshteinDistance(string s1, string s2)
        {
            int[,] distance = new int[s1.Length + 1, s2.Length + 1];

            for (int i = 0; i <= s1.Length; i++)
            {
                for (int j = 0; j <= s2.Length; j++)
                {
                    if (i == 0)
                        distance[i, j] = j;
                    else if (j == 0)
                        distance[i, j] = i;
                    else
                        distance[i, j] = Math.Min(
                            Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1),
                            distance[i - 1, j - 1] + (s1[i - 1] == s2[j - 1] ? 0 : 1)
                        );
                }
            }

            return distance[s1.Length, s2.Length];
        }

        public List<string> getStringList(string inputStr)
        {
            string[] resultArray = inputStr.Split(',');

            List<string> resultList = new List<string>(resultArray.Length);

            foreach (var item in resultArray)
            {
                resultList.Add(item.Trim());
            }

            return resultList;
        }

        public T DeepClone<T>(T source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            // Serialize the source object to a JSON string
            string jsonString = JsonSerializer.Serialize(source);

            // Deserialize the JSON string to create a new object
            T clonedObject = JsonSerializer.Deserialize<T>(jsonString);

            return clonedObject;
        }
    }
}
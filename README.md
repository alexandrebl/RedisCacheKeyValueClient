# RedisCacheKeyValueClient
Redis cache key value client manager

<strong>Develop branch</strong><br />
<img src="https://ci.appveyor.com/api/projects/status/github/alexandrebl/RedisCacheKeyValueClient?branch=develop&svg=true" alt="Project Badge" with="300">

<strong>Master branch</strong><br />
<img src="https://ci.appveyor.com/api/projects/status/github/alexandrebl/RedisCacheKeyValueClient?branch=master&svg=true" alt="Project Badge" with="300">

Hot to use:

Package console: Install-Package RedisCacheKeyValueClient

```cs
using System;
using RedisCacheKeyValueClient.Manager;

namespace RedisCacheTest {
    class Program {

        /// <summary>
        /// Main method
        /// </summary>
        static void Main() {
            //Initializing connection
            var redisCacheManager = new RedisManager<string>("127.0.0.1:6379");

            //Set first value
            redisCacheManager.SetKey("Guitar", "Ibanez RG350DX");
            //Set second value
            redisCacheManager.SetKey("Pedal", "Metal Zone");

            //Get first value
            Console.WriteLine($"Value1: {redisCacheManager.GetValue("Guitar")}");
            //Get second value
            Console.WriteLine($"Value2: {redisCacheManager.GetValue("Pedal")}");

            //Waitng for key pressed
            Console.ReadLine();
        }
    }
}
```

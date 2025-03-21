using System.Text.Json;
using System.Text.Json.Nodes;

using Lab03;


var tweets = Utils.ReadFromFile("../../../favorite-tweets.jsonl");
Console.WriteLine($"Tweets count: {tweets.Count}");

// Utils.WriteToFile("../../../favorite-tweets.xml", tweets);
// var tweets = Utils.ReadFromFile("../../../favorite-tweets.xml");
void print(List<Tweet> tweets)
{
    foreach (var tweet in tweets)
    {
        Console.WriteLine(tweet);
    }
}

void printkv<T>(List<KeyValuePair<string, T>> kvps)
{
    foreach (var kvp in kvps)
    {
        Console.WriteLine(kvp.Key + ": " + kvp.Value);
    }
}

void printkvt(List<KeyValuePair<string, List<Tweet>>> kvps)
{
    foreach (var kvp in kvps)
    {
        Console.WriteLine(kvp.Key);
        foreach (var tweet in kvp.Value)
        {
            Console.WriteLine(tweet);
        }
    }
}

Console.WriteLine("\n3\n");
print(Utils.SortByUserName(tweets).Take(5).ToList());
print(Utils.SortByCreationDate(Utils.ExtractTweetsOfUser(tweets, "NicholsUprising")).Take(5).ToList());
Console.WriteLine("\n4\n");
print(Utils.SortByCreationDate(tweets).Take(1).ToList());
print(Utils.SortByCreationDate(tweets).TakeLast(1).ToList());
Console.WriteLine("\n5\n");
printkvt(Utils.ExtractAllTweetsOfUsers(tweets).Take(1).ToList());
Console.WriteLine("\n6\n");
printkv(Utils.GetWordFrequencyOfTweets(tweets).Take(5).ToList());
Console.WriteLine("\n7\n");
printkv(Utils.SortKvPairByValue(Utils.RemoveWordsShorterThan(Utils.GetWordFrequencyOfTweets(tweets), 5).ToList()).Take(10).ToList());
Console.WriteLine("\n8\n");
printkv(Utils.SortKvPairByValue(Utils.CalculateIdf(tweets).ToList()).TakeLast(10).ToList());
Utils.WriteXML("../../../favorite-tweets.xml", tweets);
var xmlt = Utils.ReadXML("../../../favorite-tweets.xml");
print(xmlt.TakeLast(5).ToList());
using System.IO.Enumeration;
using System.Xml.Serialization;

namespace Lab03;

public class Utils
{
    public static List<Tweet> SortByUserName(List<Tweet> tweets)
    {
        return tweets.OrderBy(t => t.UserName).ToList();
    }
    
    public static List<Tweet> SortByCreationDate(List<Tweet> tweets)
    {
        return tweets.OrderBy(t => t.CreatedAt).ToList();
    }

    public static List<Tweet> ExtractTweetsOfUser(List<Tweet> tweets, string username)
    {
        return tweets.Where(t => t.UserName == username).ToList();
    }

    public static List<string> ExtractAllUsernamesOfTweets(List<Tweet> tweets)
    {
        return tweets.Select(t => t.UserName).Distinct().ToList();
    }

    public static Dictionary<string, List<Tweet>> ExtractAllTweetsOfUsers(List<Tweet> tweets)
    {
        var usernames = ExtractAllUsernamesOfTweets(tweets);
        return usernames.ToDictionary(username => username, username => tweets.Where(t => t.UserName == username).ToList());
    }
    
    public static Dictionary<string, int> GetWordFrequencyOfTweets(List<Tweet> tweets)
    {
        var wordFrequency = new Dictionary<string, int>();
        foreach (var word in tweets.Select(tweet => tweet.Text
                     .Split(new[] { ' ', '\t', '\n', '\r', '.', ',', ';', '!', '?', '-', '(', ')', ':', '"', '[', ']', '{', '\'', '}', '\\', '/' }, StringSplitOptions.RemoveEmptyEntries)
                     .Select(word => word.ToLower())
                     .ToList()).SelectMany(words => words))
        {
            wordFrequency[word] = wordFrequency.TryGetValue(word, out var value) ? value + 1 : 1;
        }
        return wordFrequency;
    }

    public static Dictionary<string, int> GetWordFrequencyOfTweet(Tweet tweet)
    {
        var wordFrequency = new Dictionary<string, int>();
        foreach (var word in tweet.Text
                     .Split(
                         new[]
                         {
                             ' ', '\t', '\n', '\r', '.', ',', ';', '!', '?', '-', '(', ')', ':', '"', '[', ']', '{',
                             '\'', '}', '\\', '/'
                         }, StringSplitOptions.RemoveEmptyEntries)
                     .Select(word => word.ToLower())
                     .ToList())
        {
            wordFrequency[word] = wordFrequency.TryGetValue(word, out var value) ? value + 1 : 1;
        }
        return wordFrequency;
    }

    public static Dictionary<string, int> RemoveWordsShorterThan(Dictionary<string, int> wordFrequency, int length)
    {
        return wordFrequency.Where(wf => wf.Key.Length < length).ToDictionary(wf => wf.Key, wf => wf.Value);
    }

    public static List<KeyValuePair<string, T>> SortKvPairByValue<T>(List<KeyValuePair<string, T>> kvps)
    {
        return kvps.OrderBy(kvp => kvp.Value).ToList();
    }

    public static Dictionary<string, int> CalculateTf(Tweet tweet)
    {
        var fd = GetWordFrequencyOfTweet(tweet);
        var sum = fd.Values.Sum();
        return fd.ToDictionary(kvp => kvp.Key, kvp => kvp.Value / sum);
    }

    public static List<Tweet> GetTweetsWithWord(List<Tweet> tweets, string word)
    {
        return tweets.Where(tweet => tweet.Text.Contains(word)).ToList();
    }

    public static Dictionary<string, double> CalculateIdf(List<Tweet> tweets)
    {
        var fd = GetWordFrequencyOfTweets(tweets);
        var idfDict = new Dictionary<string, double>();
        foreach (var kvp in fd)
        {
            var tmp = (1 + tweets.Count) / (1 + GetTweetsWithWord(tweets, kvp.Key).Count);
            idfDict.Add(kvp.Key, Math.Log(tmp));
        }
        return idfDict;
    }

    public static List<Tweet> ReadFromFile(string filepath)
    {
        var tweets = new List<Tweet>();
        var extension = Path.GetExtension(filepath);
        using var r = new StreamReader(filepath);
        while (!r.EndOfStream)
        {
            var tweetStr = r.ReadLine();
            if (tweetStr == null) continue;
            switch (extension)
            {
                case ".jsonl":
                case ".json":
                    tweets.Add(Tweet.FromJson(tweetStr));
                    break;
                case ".xml":
                    tweets.Add(Tweet.FromXml(tweetStr));
                    break;
                default:
                    throw new Exception("Unrecognized file extension: " + extension);
            }
        }
        return tweets;
    }

    public static void WriteXML(string filepath, List<Tweet> tweets)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<Tweet>));
        using var w = new StreamWriter(filepath);
        serializer.Serialize(w, tweets);
    }
    public static List<Tweet> ReadXML(string filepath)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<Tweet>));
        using var r = new StreamReader(filepath);
        return (List<Tweet>) serializer.Deserialize(r);
    }

    public static void WriteToFile(string filepath, List<Tweet> tweets)
    {
        var extension = Path.GetExtension(filepath);
        using var w = new StreamWriter(filepath);
        foreach (var tweet in tweets)
        {
            switch (extension)
            {
                case ".jsonl":
                case ".json":
                    w.WriteLine(tweet.ToJson());
                    break;
                case ".xml":
                    w.WriteLine(tweet.ToXml());
                    break;
                default:
                    throw new Exception("Unrecognized file extension: " + extension);
            }
            w.WriteLine(tweet.ToJson());
        }
    }
}
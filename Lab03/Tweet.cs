using System.Globalization;
using System.Text.Json;
using System.Xml.Serialization;

namespace Lab03;

public record StringTweet
{
    public string Text { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
    public string LinkToTweet { get; init; } = string.Empty;
    public string FirstLinkUrl { get; init; } = string.Empty;
    public DateTime CreationDateTime;
    public string CreatedAt
    {
        get => CreationDateTime.ToString("yyyy-MM-dd HH:mm:ss");
        init
        {
            const string pattern = "MMMM dd, yyyy 'at' hh:mmtt";
            var dt = DateTime.ParseExact(value.ToString(CultureInfo.InvariantCulture), pattern, CultureInfo.InvariantCulture);
            CreationDateTime = dt;
        }
    }
    public string TweetEmbedCode { get; init; } = string.Empty;
}

public class Tweet
{
    public string Text { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
    public string LinkToTweet { get; init; } = string.Empty;
    public string FirstLinkUrl { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; } = DateTime.MinValue;
    public string TweetEmbedCode { get; init; } = string.Empty;

    private Tweet(StringTweet tweet)
    {
        Text = tweet.Text;
        UserName = tweet.UserName;
        LinkToTweet = tweet.LinkToTweet;
        FirstLinkUrl = tweet.FirstLinkUrl;
        CreatedAt = tweet.CreationDateTime;
        TweetEmbedCode = tweet.TweetEmbedCode;
    }

    public static Tweet FromJson(string jsonString)
    {
        var st = JsonSerializer.Deserialize<StringTweet>(jsonString);
        return new Tweet(st ?? throw new InvalidOperationException("Unable to deserialize"));
    }
    
    public static Tweet FromXml(string xmlString)
    {
        var xmlSerializer = new XmlSerializer(typeof(Tweet));
        using var sr = new StringReader(xmlString);
        var st = (StringTweet)xmlSerializer.Deserialize(sr)!;
        return new Tweet(st ?? throw new InvalidOperationException("Unable to deserialize"));
    }

    public string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }

    public string ToXml()
    {
        var xmlSerializer = new XmlSerializer(typeof(Tweet));

        using var stringWriter = new StringWriter();
        xmlSerializer.Serialize(stringWriter, this);
        return stringWriter.ToString();
    }

    public Tweet()
    {
    }

    public override string ToString()
    {
        return $"{UserName} at {CreatedAt}" +
               $"\n\tText: {Text}";
    }
};
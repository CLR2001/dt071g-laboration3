using System.Text.Json;

public class JsonPostStorage(string filename) : IPostStorage
{
  public List<Post> Load()
  {
    if (!File.Exists(filename)) return new List<Post>();

    string jsonString = File.ReadAllText(filename);

    return JsonSerializer.Deserialize<List<Post>>(jsonString) ?? new List<Post>();
  }

  public void Save(List<Post> posts)
  {
    string jsonString = JsonSerializer.Serialize(posts);
    File.WriteAllText(filename, jsonString);
  }
}
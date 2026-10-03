// Klass för ett gästbok objekt som hanterar lagring och radering av inlägg
public class Guestbook(IPostStorage storage)
{
  private readonly List<Post> _posts = storage.Load();

  public List<Post> GetPosts() => _posts;

  public Post AddPost(string owner, string content)
  {
    Post post = new() { Owner = owner, Content = content };

    _posts.Add(post);
    storage.Save(_posts);

    return post;
  }

  public bool DeletePost(int index)
  {
    if (index >= 0 && index < _posts.Count)
    {
      _posts.RemoveAt(index);
      storage.Save(_posts);
      return true;
    }
    else
    {
      return false;
    }
  }
}
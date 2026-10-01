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

  public bool DeletePost(Post post)
  {
    bool removed = _posts.Remove(post);
    if (removed) storage.Save(_posts);
    return removed;
  }
}
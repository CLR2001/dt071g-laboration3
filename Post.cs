// Klass för ett inlägg som innehåller en lätt felhantering genom att inte släppa igenom null eller whitespace.
public class Post
{
  private string _owner = string.Empty;
  private string _content = string.Empty;

  public required string Owner
  {
    get => _owner;
    set => _owner = !string.IsNullOrWhiteSpace(value)
      ? value
      : throw new ArgumentException("Användarens namn får inte vara tomt!");
  }

  public required string Content
  {
    get => _content;
    set => _content = !string.IsNullOrWhiteSpace(value)
      ? value
      : throw new ArgumentException("Inläggets innehåll får inte vara tomt!");
  }
}
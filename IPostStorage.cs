public interface IPostStorage
{
  List<Post> Load();
  void Save(List<Post> posts);
}
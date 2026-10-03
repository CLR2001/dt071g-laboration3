/*
 * Skapare: Ludvig Rosenqvist
 * Kurskod: DT071G
 * Uppgift: Laboration 3
 * Datum: 2026-10-02
 * Beskrivning: Ett konsolprogram som tillåter en användare att skriva och radera inlägg i en gästbok. Alla inlägg sparas i en extern JSON-fil.
 */

IPostStorage storage = new JsonPostStorage("guestbook.json");
var guestbook = new Guestbook(storage);

while (true)
{
  Console.Clear();

  PrintMenu(guestbook);
  var userInput = Console.ReadKey(intercept: true);

  switch (char.ToLower(userInput.KeyChar))
  {
    case '1':
      // Skriver ut gränssnittet och kör det tills användaren är klar och boolean blir true
      bool isAdding = true;
      while (isAdding)
      {
        Console.Clear();
        Console.WriteLine("────────────────────────────────────────────────");
        Console.WriteLine("               Lägg till inlägg                 ");
        Console.WriteLine("────────────────────────────────────────────────");
        Console.WriteLine("(Tryck på Esc när som helst för att avbryta)\n");

        Console.Write("Namn:");
        var userName = ReadLineOrExit();

        // Accepterar namn om det inte är null eller whitespace
        if (string.IsNullOrWhiteSpace(userName))
        {
          Console.WriteLine("\nNamn får inte vara tomt! Tryck på valfri tangent för att försöka igen...");
          Console.ReadKey(intercept: true);
          continue;
        }

        // Accepterar meddelande om det inte är null eller whitespace
        Console.Write("Meddelande:");
        var message = ReadLineOrExit();
        if (string.IsNullOrWhiteSpace(message))
        {
          Console.WriteLine("\nMeddelande får inte vara tomt! Tryck på valfri tangent för att försöka igen...");
          Console.ReadKey(intercept: true);
          continue;
        }

        // Om allt är korrekt sparas inlägget
        guestbook.AddPost(userName, message);
        Console.WriteLine("\nMeddelandet lades till i gästboken!");
        Console.WriteLine("(Tryck på valfri tangent för att fortsätta...)");
        Console.ReadKey(intercept: true);
        isAdding = false;
      }

      break;

    case '2':
      // Skriver ut gränssnittet och kör det tills användaren är klar och boolean blir true
      bool isDeleting = true;
      while (isDeleting)
      {
        Console.Clear();
        Console.WriteLine("────────────────────────────────────────────────");
        Console.WriteLine("              Radera ett inlägg                 ");
        Console.WriteLine("────────────────────────────────────────────────");
        Console.WriteLine("(Tryck på Esc när som helst för att avbryta)");
        Console.WriteLine();
        Console.WriteLine("────────────────────────────────────────");
        Console.WriteLine("  INLÄGG:");
        Console.WriteLine("────────────────────────────────────────");

        var posts = guestbook.GetPosts();
        for (int i = 0; i < posts.Count; i++)
        {
          Console.WriteLine($"  [{i}] {posts[i].Owner} - {posts[i].Content}");
        }

        Console.WriteLine("────────────────────────────────────────");
        Console.WriteLine();
        Console.Write("Välj ett inlägg att ta bort (index):");

        var indexToDelete = ReadLineOrExit();

        // Kollar vilket värde ReadLineOrExit returnerar och låter användaren antingen försöka igen eller raderar inlägget efter ett giltligt index.
        if (indexToDelete == null)
        {
          break;
        }

        if (int.TryParse(indexToDelete, out int index) && index >= 0 && index < posts.Count)
        {
          guestbook.DeletePost(index);
          Console.WriteLine($"\nInlägget med index {index} raderades.");
          Console.WriteLine("(Tryck på valfri tangent för att fortsätta...)");
          Console.ReadKey(intercept: true);
          isDeleting = false;
        }
        else
        {
          Console.WriteLine("\nOgiltigt index! Tryck på valfri tangent för att försöka igen...");
          Console.ReadKey(intercept: true);
        }
      }

      break;

    case 'x':
      Console.WriteLine("\nAvslutar programmet...");
      return;

    default:
      Console.WriteLine("\nOgiltligt val, tryck på valfri tangent för att försöka igen...");
      Console.ReadKey(intercept: true);
      ClearLastLine();
      break;
  }

}

// Funktion för att skriva ut huvudmeny-gränssnittet och alla lagrade inlägg
static void PrintMenu(Guestbook guestbook)
{
  Console.WriteLine("════════════════════════════════════════");
  Console.WriteLine("     L U D V I G ' S   G Ä S T B O K");
  Console.WriteLine("════════════════════════════════════════");
  Console.WriteLine();
  Console.WriteLine("  1. Skriv i gästboken");
  Console.WriteLine("  2. Ta bort inlägg");
  Console.WriteLine();
  Console.WriteLine("  X. Avsluta");
  Console.WriteLine();
  Console.WriteLine("────────────────────────────────────────");
  Console.WriteLine("  INLÄGG:");
  Console.WriteLine("────────────────────────────────────────");

  var posts = guestbook.GetPosts();
  for (int i = 0; i < posts.Count; i++)
  {
    Console.WriteLine($"  [{i}] {posts[i].Owner} - {posts[i].Content}");
  }

  Console.WriteLine("────────────────────────────────────────");
  Console.WriteLine();
  Console.Write("Välj ett alternativ: ");
}

// Funktion för att rensa sista raden i konsolen
static void ClearLastLine()
{
  // Flytta upp markören till raden ovanför
  Console.SetCursorPosition(0, Console.CursorTop - 1);

  // Skriv över raden med blanksteg
  Console.Write(new string(' ', Console.WindowWidth));

  // Flytta tillbaka markören till raden ovanför igen
  Console.SetCursorPosition(0, Console.CursorTop - 1);
}

// Funktion som lyssnar på senaste tangenttryck och returnerar olika värden beroende på vilka tangenter som trycks
static string? ReadLineOrExit()
{
  var input = new System.Text.StringBuilder();
  while (true)
  {

    var pressedKey = Console.ReadKey(intercept: true);

    // Kollar om esc trycks och returnerar null i så fall
    if (pressedKey.Key == ConsoleKey.Escape)
    {
      return null;
    }
    
    // Kollar om enter trycks och returnerar strängen/input
    if (pressedKey.Key == ConsoleKey.Enter)
    {
      Console.WriteLine();
      return input.ToString();
    }

    // Kollar om backspace trycks och raderar senaste tecknet från strängen
    if (pressedKey.Key == ConsoleKey.Backspace && input.Length > 0)
    {
      input.Remove(input.Length - 1, 1);
      Console.Write("\b \b");
    }

    // Om en ett tecken som inte är en control-tangent trycks läggs det till i strängen
    else if (!char.IsControl(pressedKey.KeyChar))
    {
      input.Append(pressedKey.KeyChar);
      Console.Write(pressedKey.KeyChar);
    }
  }
}
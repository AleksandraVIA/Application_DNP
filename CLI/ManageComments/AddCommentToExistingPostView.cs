
namespace CLI.UI
{
    public class AddCommentToExistingPostView
    {
        public string GetCommentBody()
        {
            Console.Write("Enter comment body: ");
            return Console.ReadLine();
        }

        public string GetUserId()
        {
            Console.Write("Enter user ID: ");
            return Console.ReadLine();
        }

        public string GetPostId()
        {
            Console.Write("Enter post ID: ");
            return Console.ReadLine();
        }

        public void ShowSuccessMessage()
        {
            Console.WriteLine("Comment added successfully.");
        }

        public void ShowErrorMessage(string message)
        {
            Console.WriteLine($"Error: {message}");
        }
    }
}
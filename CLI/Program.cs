using CLI.UI;


using FileRepositories;
using RepositoryContracts;

        
    
    Console.WriteLine("Starting CLI...");
    IUserRepository userRepository = new UserFileRepository();
    IPostRepository postRepository = new PostFileRepository();
    ICommentRepository commentRepository = new CommentFileRepository();

    CliApp cliApp = new CliApp(userRepository, commentRepository, postRepository);
    await cliApp.StartAsync();
    
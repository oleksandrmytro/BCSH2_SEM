using BCSH2_SEM.Commands;
using BCSH2_SEM.Model;

namespace BCSH2_SEM.ViewModel;

public class LoginVM
{
    private User user;

    public User User
    {
        get { return user; } 
        set { user = value; }
    } 
    
    public RegisterCommand RegisterCommand { get; set; }
    public LoginCommand LoginCommand { get; set; }
    
    public LoginVM()
    {
        RegisterCommand = new RegisterCommand(this);
        LoginCommand = new LoginCommand(this);
    }

}
using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FirebaseWrapper
{
    public class Auth
    {
        public static async Task<Firebase.Database.FirebaseClient> AuthorizedDatabaseAsync(String DatabaseURL, String APIKey, String Email, String Password) 
        {
            try
            {
                FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(APIKey));
                FirebaseAuthLink auth = await authProvider.SignInWithEmailAndPasswordAsync(Email, Password);
                FirebaseOptions options = new FirebaseOptions();
                options.AuthTokenAsyncFactory = () => Task.FromResult(auth.FirebaseToken);
                return new FirebaseClient(DatabaseURL, options);
            }
            catch (Exception e)
            {
                return null;
            }
        }        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObligOppgave2
{
    public class User
    {
        public int Id { get; }
        public string Name { get; }
        public string Email { get; set; }
        public int Role { get; }
        private string Password;
        public User (int id, string name, string email, int role, string password)
        {
            Id = id;
            Name = name;
            Email = email;
            Password = password;
            Role = role;
        }

        public User (string name, string email, int role, string password)
        {
            Name = name;
            Email = email;
            Role = role;
            Password = password;
        }
        public User(string name, string email, int role)
        {
            Name = name;
            Email = email;
            Role = role;
            Id = -1;
            Password = String.Empty;
        }

        public void EditEpost(string newEmail)
        {
            Email = newEmail;
        }

        public bool Login(string password)
        {
            if(Password == password)
            {
                return true;
            }
            return false;
        }
    }
}

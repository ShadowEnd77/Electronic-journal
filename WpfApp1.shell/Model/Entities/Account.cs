using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.shell.Model.Entities
{
    public class Account
    {
        public int IdAccount { get; set; } // id_аккаунта
        public string Login { get; set; } // Логин
        public string Password { get; set; } // Пароль

        public Teacher Teacher { get; set; } // Связь 1:1 с Учителя
        public Student Student { get; set; } // Связь 1:1 с Ученики
    }
}

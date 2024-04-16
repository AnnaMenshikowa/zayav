// RegAndAuth.cs
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.Services.Client;
using System.Linq;

namespace KP_Staff.Actions
{
    class RegAndAuth
    {
        public static WorkersEntities1 workersEntities { get; set; }
        public static ObservableCollection<Staff> ListStaff { get; set; }
        public static ObservableCollection<Challenger> ListChallenger { get; set; }
        public RegAndAuth() 
        {
            workersEntities = new WorkersEntities1();
            ListStaff = new ObservableCollection<Staff>();
            ListChallenger = new ObservableCollection<Challenger>();
        }

        public bool AddUser(string role, string log, string pass)
        {
            bool status = false;
            if(CheckUserReg(role, log) == false)
            {
                if (role == "Работник")
                {
                    Staff staff = new Staff();
                    staff.login = log;
                    staff.password = pass;
                    staff.middlename = "не задано";
                    staff.phone = "не задано";
                    staff.number_p = 0;
                    staff.series_p = 0;
                    staff.surname = "не задано";
                    staff.email = "не задано";
                    staff.C_name = "не задано";

                    try
                    {                  
                        workersEntities.Staff.Add(staff);
                        ListStaff.Add(staff);
                        System.Windows.MessageBox.Show("Работник успешно зарегистрирован!");
                        status = true;

                        try
                        {
                            workersEntities.SaveChanges();
                        }
                        catch (DbEntityValidationException ex)
                        {
                            foreach (DbEntityValidationResult validationError in ex.EntityValidationErrors)
                            {
                                System.Windows.MessageBox.Show("Object: " + validationError.Entry.Entity.ToString());
                                System.Windows.MessageBox.Show(" ");
                                foreach (DbValidationError err in validationError.ValidationErrors)
                                {
                                    System.Windows.MessageBox.Show(err.ErrorMessage + "");                        
                                }
                            }
                        }
                    }
                    catch (DataServiceRequestException ex)
                    {
                        throw new ApplicationException("Ошибка добавления пользователя" + ex.ToString());
                    }
                }
                else if (role == "Претендент")
                {
                    Challenger challenger = new Challenger();
                    challenger.login = log;
                    challenger.password = pass;
                    challenger.middlename = "не задано";
                    challenger.phone = "не задано";
                    challenger.surname = "не задано";
                    challenger.email = "не задано";
                    challenger.C_name = "не задано";
                    challenger.C_address = "не задано";
                    try
                    {
                        workersEntities.Challenger.Add(challenger);
                        ListChallenger.Add(challenger);
                        System.Windows.MessageBox.Show("Претендент успешно зарегистрирован!");
                        status = true;
                        workersEntities.SaveChanges();
                    }
                    catch (DataServiceRequestException ex)
                    {
                        throw new ApplicationException("Ошибка добавления пользователя" + ex.ToString());
                    }
                }
            }
            return status;
        }

        public bool CheckUserReg(string role, string log)
        {
            bool result = false;
            bool result1 = false;
            bool result2 = false;
            ListStaff.Clear();
            ListChallenger.Clear();

            if (role == "Работник")
            {
                var queryEmployee = from staff in workersEntities.Staff where staff.login == log select staff;

                foreach (Staff st in queryEmployee)
                {
                    ListStaff.Add(st);
                }

                if (ListStaff.Count > 0)
                {
                    result1 = true;
                }         
            }
            if (role == "Претендент")
            {
                var queryEmployee = from challeger in workersEntities.Challenger where challeger.login == log select challeger;
                foreach (Challenger ch in queryEmployee)
                {
                    ListChallenger.Add(ch);
                }

                if (ListChallenger.Count > 0)
                {
                    result2 = true;
                }
            }

            if(result1 == true || result2 == true)
            {
                result = true;
                System.Windows.MessageBox.Show("Пользователь с таким логином уже существует. Введите другой логин или перейдите к Авторизации.");
            }
            else
            {
                result = false;
            }
            return result;
        }

        public int AuthUser(string log, string pass)
        {
            int status;
            if (CheckUserRole(log, pass) == "Работник")
            {
                status = 1;
            }
            else if (CheckUserRole(log, pass) == "Претендент")
            {
                status = 2;
            }

            else
            {
                status = 0;
            }
            return status;
        }

        public string CheckUserRole(string log, string pass)
        {
            bool result = false;
            string role = "";
            ListStaff.Clear();
            ListChallenger.Clear();
            var queryEmployee1 = from staff in workersEntities.Staff
                                where staff.login == log && staff.password == pass
                                select staff;

            foreach (Staff st in queryEmployee1)
            {
                ListStaff.Add(st);
            }
            if (ListStaff.Count >= 1)
            {
                result = true;
                role = "Работник";
            }

            var queryEmployee2 = from challenger in workersEntities.Challenger
                                 where challenger.login == log && challenger.password == pass
                                 select challenger;
            foreach (Challenger ch in queryEmployee2)
            {
                ListChallenger.Add(ch);
            }
           
            if (ListChallenger.Count >= 1)
            {
                result = true;
                role = "Претендент";                
            }
           
            return role;
        }

        public bool AddUserInfo(string role, string log, string pass, string name, string middlename, string surname, string edu, DateTime birth, int series_p, int num_p, DateTime start, string email, string phone, string addr)
        {
            bool status = false;
            var staff1 = workersEntities.Staff.Where(c => c.login == log && c.password == pass).FirstOrDefault();
            var challenger1 = workersEntities.Challenger.Where(c => c.login == log && c.password == pass).FirstOrDefault();

            if (role == "Работник")
            {
                staff1.login = log;
                staff1.password = pass;
                staff1.middlename = middlename;
                staff1.phone = phone;
                staff1.number_p = num_p;
                staff1.series_p = series_p;
                staff1.surname = surname;
                staff1.email = email;
                staff1.C_name = name;
                staff1.education = edu;
                staff1.dateofbirth = birth;
                staff1.dateofstartjob = start;
                workersEntities.SaveChanges();
                status = true;
            }
            if (role == "Претендент")
            {
                challenger1.login = log;
                challenger1.password = pass;
                challenger1.middlename = middlename;
                challenger1.phone = phone;
                challenger1.surname = surname;
                challenger1.email = email;
                challenger1.C_name = name;
                challenger1.C_address = addr;
                challenger1.education = edu;
                challenger1.dateofbirth = birth;
                workersEntities.SaveChanges();
                status = true;
            }
            return status;
        }
    }
}

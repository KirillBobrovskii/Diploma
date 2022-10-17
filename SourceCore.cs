using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScholarshipAppointment.DataBase;

namespace ScholarshipAppointment
{
    public class SourceCore
    {
        //Конструктор
        private SourceCore() { }

        public static DataBase.Users currentUser; //Хранение идентификатора текущего пользователя

        //Экземпляр класса Bobrovski_4IS1_DataBase_ScholarshipAppointmentEntitie для получения базы данных с сервера
        private static Bobrovski_4IS1_DataBase_ScholarshipAppointment2Entities MyDataBase;

        //Инициализация экземпляра класса Bobrovski_4IS1_DataBase_ScholarshipAppointmentEntitie (получение базы данных)
        public static Bobrovski_4IS1_DataBase_ScholarshipAppointment2Entities getBase()
        {
            if (MyDataBase == null)
            {
                return MyDataBase = new Bobrovski_4IS1_DataBase_ScholarshipAppointment2Entities();
            }
            return MyDataBase;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookingticketAPI.StatusConstants
{
    public class StatusCodeConstants
    {
            public static int ERROR_SERVER = 500;
            public static int BAD_REQUEST = 400;
            public static int NOT_FOUND = 404;
            public static int AUTHORIZATION = 401;
            public static int FORBIDDEN = 403;
            public static int CREATED = 201;
            public static int OK = 200;

          

    }
    public class Config
    {
        public static string hostName = "https://movieapi.cybersoft.edu.vn/hinhanh/";
        public static string connect = "Server=103.97.125.205,1433;database=dbRapChieuPhim;user id = khaicybersoft; password=khaicybersoft321@;Trusted_Connection=True;Integrated Security = False";
    }
}

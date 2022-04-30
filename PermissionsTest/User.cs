using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PermissionsTest
{
    class User
    {
        public static List<User> Users = new List<User>() 
        { 
            new User() {
                username = "blake",
                password = "highlydedicated",
                permissions = 0x06 },
            new User() {
                username = "shea",
                password = "highlydedicated",
                permissions = 0x03 },
            new User() {
                username = "admin",
                password = "highlydedicated",
                permissions = 0x09 }
        };

        public string username;
        public string password;
        public ulong permissions;
    }

    public class Permission
    {
        public static List<Permission> Permissions = new List<Permission>()
        {
            new Permission() {
                name = "button1",
                code = 0x01 },
            new Permission() {
                name = "button2",
                code = 0x02 },
            new Permission() {
                name = "button3",
                code = 0x04 },
            new Permission() {
                name = "button4",
                code = 0x08 }
        };

        public static ulong[] ParsePermissions(ulong combinedPermissions)
        {
            List<ulong> temp = new List<ulong>();
            IOrderedEnumerable<Permission> ordered = Permissions.OrderByDescending(o => o.code);

            foreach (Permission p in ordered)
            {
                if (p.code <= combinedPermissions)
                {
                    temp.Add(p.code);
                    combinedPermissions -= p.code;
                }
            }

            return temp.ToArray();
        }

        public static bool hasPermission(string username, ulong permission)
        {
            if (ParsePermissions(User.Users.Find(el => el.username == username).permissions).Contains(permission))
                return true;
            else
                return false;
        }
        
        public static int PermissionComparison(Permission p1, Permission p2)
        {
            if (p1.code > p2.code)
                return (int)p1.code;
            else
                return (int)p2.code;
        }

        public string name;
        public ulong code;
    }
}

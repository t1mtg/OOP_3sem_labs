using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Text;
using DTO;
using DTO.Exceptions;
using Newtonsoft.Json;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            /*GetAllEmployees();
            GetAllTasks();
            AddNewCommentToTask(Guid.Parse("b6ad292e-700a-469e-9854-4ce91d8c5b1a"), "secondComment", Guid.Parse("af5bec34-0024-4a5e-91be-651af5e4bd23"));
            ChangeTaskStatus(Guid.Parse("b6ad292e-700a-469e-9854-4ce91d8c5b1a"), TaskStatusType.Active, Guid.Parse("af5bec34-0024-4a5e-91be-651af5e4bd23"));
            ChangeTaskEmployee(Guid.Parse("b6ad292e-700a-469e-9854-4ce91d8c5b1a"), Guid.Parse("622ccc94-676d-4b30-b64d-154c60cdb4d4"), Guid.Parse("af5bec34-0024-4a5e-91be-651af5e4bd23"));
            FindTaskById(Guid.Parse("3d745c8d-7633-4174-905a-50cbf63f5027"));
            AddNewTask(Guid.Parse("622ccc94-676d-4b30-b64d-154c60cdb4d4"), "timofeys one");
            FindTaskByEditTime(DateTime.Now);
            FindTaskByEmployee(Guid.Parse("af5bec34-0024-4a5e-91be-651af5e4bd23"));
            AddNewCommentToTask(Guid.Parse("2ccfc07c-fe9a-44b2-81e6-0d304cb5060f"),"comment by tim", Guid.Parse("622ccc94-676d-4b30-b64d-154c60cdb4d4"));
            FindTaskByEmployeeEdit(Guid.Parse("622ccc94-676d-4b30-b64d-154c60cdb4d4"));
            //ChangeTaskStatus(Guid.Parse("34f85195-c9e4-489e-ba3a-ed35e6a7d323"), TaskStatusType.Active, Guid.Parse("622ccc94-676d-4b30-b64d-154c60cdb4d4"));
            UpdateLeader(Guid.Parse("af5bec34-0024-4a5e-91be-651af5e4bd23"),
                Guid.Parse("622ccc94-676d-4b30-b64d-154c60cdb4d4"));
            FindSubordinateTasks(Guid.Parse("622ccc94-676d-4b30-b64d-154c60cdb4d4"));*/
        }
        

        private static void HireEmployee(string name)
        {
            var request = WebRequest.Create($"https://localhost:5001/employees/hireEmployee/?name={name}");
            request.Method = WebRequestMethods.Http.Post;
            WebResponse response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            if (responseStream == null) throw new NoResponseFromServerException();
            using var readStream = new StreamReader(responseStream, Encoding.UTF8);
            string responseString = readStream.ReadToEnd();
            Employee employee = JsonConvert.DeserializeObject<Employee>(responseString);

            Console.WriteLine("Employee successfully created.");
            Console.WriteLine($"Id: {employee.Id}, Name: {employee.Name}");
        }
        
        private static void FireEmployee(Guid employeeId)
        {
            var request = WebRequest.Create($"https://localhost:5001/employees/fireEmployee/?employeeId={employeeId}");
            request.Method = WebRequestMethods.Http.Post;
            WebResponse response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            if (responseStream == null) throw new NoResponseFromServerException();
            using var readStream = new StreamReader(responseStream, Encoding.UTF8);
            string responseString = readStream.ReadToEnd();
            HttpStatusCode httpStatusCode = JsonConvert.DeserializeObject<HttpStatusCode>(responseString);
            Console.WriteLine($"Employee successfully fired. Return status: " + httpStatusCode);
        }

        private static Employee FindEmployeeById(Guid id)
        {
            var request = WebRequest.Create($"https://localhost:5001/employees/getEmployeeById/?id={id}");
            request.Method = WebRequestMethods.Http.Get;

            try
            {
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                if (responseStream == null) throw new NoResponseFromServerException();
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                string responseString = readStream.ReadToEnd();
                Employee employee = JsonConvert.DeserializeObject<Employee>(responseString);
                Console.WriteLine("Found employee by id:");
                Console.WriteLine($"Id: {employee.Id}, Name: {employee.Name}");
                return employee;
            }
            catch (WebException e)
            {
                Console.WriteLine("Employee was not found");
                Console.Error.WriteLine(e.Message);
            }

            return null;
        }

        private static Employee FindEmployeeByName(string name)
        {
            var request = WebRequest.Create($"https://localhost:5001/employees/getEmployeeByName/?name={name}");
            request.Method = WebRequestMethods.Http.Get;
            try
            {
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                if (responseStream == null) throw new NoResponseFromServerException();
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                string responseString = readStream.ReadToEnd();
                Employee employee = JsonConvert.DeserializeObject<Employee>(responseString);
                Console.WriteLine("Found employee by name:");
                Console.WriteLine($"Id: {employee.Id}, Name: {employee.Name}");
            }
            catch (WebException e)
            {
                Console.WriteLine("Employee was not found");
                Console.Error.WriteLine(e.Message);
            }

            return null;
        }

        private static List<Employee> GetAllEmployees()
        {
            var request = WebRequest.Create($"https://localhost:5001/employees/getAllEmployees/");
            request.Method = WebRequestMethods.Http.Get;
            try
            {
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                if (responseStream == null) throw new NoResponseFromServerException();
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                string responseString = readStream.ReadToEnd();
                List<Employee> employees = JsonConvert.DeserializeObject<List<Employee>>(responseString);

                Console.WriteLine("List of employees:");
                foreach (Employee employee in employees)
                {
                    Console.WriteLine($"Id: {employee.Id}");
                    Console.WriteLine($"Name: {employee.Name}");
                }

                return employees;
            }
            catch (WebException e)
            {
                Console.WriteLine("Employees were not found");
                Console.Error.WriteLine(e.Message);
            }

            return null;
        }
        
        private static Employee UpdateLeader(Guid employeeId, Guid leaderId)
        {
            var request = WebRequest.Create($"https://localhost:5001/employees/updateLeader/?employeeId={employeeId}&leaderId={leaderId}");
            request.Method = WebRequestMethods.Http.Put;
            try
            {
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                if (responseStream == null) throw new NoResponseFromServerException();
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                string responseString = readStream.ReadToEnd();
                Employee employee = JsonConvert.DeserializeObject<Employee>(responseString);
                Console.WriteLine($"Leader for employee {employee.Name}, id: {employee.Id}, updated");
                Guid leader = employee.LeaderId;
                Console.WriteLine($"New leader Id: {leaderId}");
                return employee;
            }
            catch (WebException e)
            {
                Console.WriteLine("unable to update leader");
                Console.Error.WriteLine(e.Message);
            }

            return null;
        }

        private static Task AddNewTask(Guid employeeId, string name)
        {
            var request = WebRequest.Create($"https://localhost:5001/tasks/addTask/?employeeId={employeeId}&name={name}");
            request.Method = WebRequestMethods.Http.Post;
            try
            {
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                if (responseStream == null) throw new NoResponseFromServerException();
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                string responseString = readStream.ReadToEnd();
                Task task = JsonConvert.DeserializeObject<Task>(responseString);
                Console.WriteLine($"New task successfully created, id:{task.Id}");
                return task;
            }
            catch (WebException e)
            {
                Console.WriteLine("unable to add task");
                Console.Error.WriteLine(e.Message);
            }

            return null;
        }

        private static void AddNewCommentToTask(Guid taskId, string comment, Guid employeeId)
        {
            var request = WebRequest.Create($"https://localhost:5001/tasks/addComment/?taskId={taskId}&comment={comment}&employeeId={employeeId}");
            request.Method = WebRequestMethods.Http.Put;
            try
            {
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                if (responseStream == null) throw new NoResponseFromServerException();
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                string responseString = readStream.ReadToEnd();
                Task task = JsonConvert.DeserializeObject<Task>(responseString);
                Console.WriteLine($"Comment for the task successfully created, task id:{task.Id}, comment: {comment}");
            }
            catch (WebException e)
            {
                Console.WriteLine("unable to add comment");
                Console.Error.WriteLine(e.Message);
            }
        }
        
        private static void ChangeTaskStatus(Guid taskId, TaskStatusType statusType, Guid employeeId)
        {
            var request = WebRequest.Create($"https://localhost:5001/tasks/changeStatus/?taskId={taskId}&taskStatusType={statusType}&employeeId={employeeId}");
            request.Method = WebRequestMethods.Http.Put;
            try
            {
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                if (responseStream == null) throw new NoResponseFromServerException();
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                string responseString = readStream.ReadToEnd();
                Task task = JsonConvert.DeserializeObject<Task>(responseString);
                Console.WriteLine($"Status of the task successfully created, task id:{task.Id}, current status: {statusType}");
            }
            catch (WebException e)
            {
                Console.WriteLine("unable to add comment");
                Console.Error.WriteLine(e.Message);
            }
        }
        
        private static void ChangeTaskEmployee(Guid taskId, Guid newEmployeeId, Guid employeeId)
        {
            var request = WebRequest.Create($"https://localhost:5001/tasks/changeTaskEmployee/?taskId={taskId}&newEmployeeId={newEmployeeId}&employeeId={employeeId}");
            request.Method = WebRequestMethods.Http.Put;
            try
            {
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                if (responseStream == null) throw new NoResponseFromServerException();
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                string responseString = readStream.ReadToEnd();
                Task task = JsonConvert.DeserializeObject<Task>(responseString);
                Console.WriteLine($"Employee for the task successfully changed, task id:{task.Id}, current employee: {newEmployeeId}");
            }
            catch (WebException e)
            {
                Console.WriteLine("unable to change employee");
                Console.Error.WriteLine(e.Message);
            }
        }
        
        
        private static List<Task> GetAllTasks()
        {
            var request = WebRequest.Create($"https://localhost:5001/tasks/allTasks/");
            request.Method = WebRequestMethods.Http.Get;
            try
            {
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                if (responseStream == null) throw new NoResponseFromServerException();
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                string responseString = readStream.ReadToEnd();
                List<Task> tasks = JsonConvert.DeserializeObject<List<Task>>(responseString);
                Console.WriteLine($"Tasks:");
                foreach (Task task in tasks)
                {
                    Console.WriteLine($"{task.Id}, {task.Name}, {task.TaskStatus}, {task.EmployeeId}, {task.CreationTime}");
                }
                return tasks;
            }
            catch (WebException e)
            {
                Console.WriteLine("0 tasks found");
                Console.Error.WriteLine(e.Message);
            }

            return null;
        }
        
        private static Task FindTaskById(Guid taskId)
        {
            var request = WebRequest.Create($"https://localhost:5001/tasks/taskById/?taskId={taskId}");
            request.Method = WebRequestMethods.Http.Get;
            try
            {
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                if (responseStream == null) throw new NoResponseFromServerException();
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                string responseString = readStream.ReadToEnd();
                Task task = JsonConvert.DeserializeObject<Task>(responseString);
                Console.WriteLine($"Task found: {task.Name}");
                return task;
            }
            catch (WebException e)
            {
                Console.WriteLine("0 tasks found");
                Console.Error.WriteLine(e.Message);
            }

            return null;
        }
        
        private static List<Task> FindTaskByCreationTime(DateTime dateTime)
        {
            var request = WebRequest.Create($"https://localhost:5001/tasks/taskByCreationTime/?dateTime={dateTime}");
            request.Method = WebRequestMethods.Http.Get;
            try
            {
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                if (responseStream == null) throw new NoResponseFromServerException();
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                string responseString = readStream.ReadToEnd();
                List<Task> tasks = JsonConvert.DeserializeObject<List<Task>>(responseString);
                Console.WriteLine("tasks with this Creation Time:");
                foreach (Task task in tasks)
                {
                    Console.WriteLine($"{task.Id}, {task.Name}");
                }
                return tasks;
            }
            catch (WebException e)
            {
                Console.WriteLine("0 tasks found");
                Console.Error.WriteLine(e.Message);
            }

            return null;
        }
        
        private static List<Task> FindTaskByEditTime(DateTime dateTime)
        {
            var request = WebRequest.Create($"https://localhost:5001/tasks/taskByEditTime/?dateTime={dateTime}");
            request.Method = WebRequestMethods.Http.Get;
            try
            {
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                if (responseStream == null) throw new NoResponseFromServerException();
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                string responseString = readStream.ReadToEnd();
                List<Task> tasks = JsonConvert.DeserializeObject<List<Task>>(responseString);
                Console.WriteLine("tasks with this Edit Time:");
                foreach (Task task in tasks)
                {
                    Console.WriteLine($"{task.Id}, {task.Name}");
                }
                return tasks;
            }
            catch (WebException e)
            {
                Console.WriteLine("0 tasks found");
                Console.Error.WriteLine(e.Message);
            }

            return null;
        }
        
        private static List<Task> FindTaskByEmployee(Guid employeeId)
        {
            var request = WebRequest.Create($"https://localhost:5001/tasks/taskByEmployee/?employeeId={employeeId}");
            request.Method = WebRequestMethods.Http.Get;
            try
            {
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                if (responseStream == null) throw new NoResponseFromServerException();
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                string responseString = readStream.ReadToEnd();
                List<Task> tasks = JsonConvert.DeserializeObject<List<Task>>(responseString);
                Console.WriteLine("tasks by this employee:");
                foreach (Task task in tasks)
                {
                    Console.WriteLine($"{task.Id}, {task.Name}");
                }
                return tasks;
            }
            catch (WebException e)
            {
                Console.WriteLine("0 tasks found");
                Console.Error.WriteLine(e.Message);
            }

            return null;
        }
        
        private static List<Task> FindTaskByEmployeeEdit(Guid employeeId)
        {
            var request = WebRequest.Create($"https://localhost:5001/tasks/taskByEmployeeEdit/?employeeId={employeeId}");
            request.Method = WebRequestMethods.Http.Get;
            try
            {
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                if (responseStream == null) throw new NoResponseFromServerException();
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                string responseString = readStream.ReadToEnd();
                List<Task> tasks = JsonConvert.DeserializeObject<List<Task>>(responseString);
                Console.WriteLine("tasks edited by this employee:");
                foreach (Task task in tasks)
                {
                    Console.WriteLine($"{task.Id}, {task.Name}");
                }
                return tasks;
            }
            catch (WebException e)
            {
                Console.WriteLine("0 tasks found");
                Console.Error.WriteLine(e.Message);
            }

            return null;
        }
        
        private static List<Task> FindSubordinateTasks(Guid employeeId)
        {
            var request = WebRequest.Create($"https://localhost:5001/tasks/subordinateTasks/?employeeId={employeeId}&employees={GetAllEmployees()}");
            request.Method = WebRequestMethods.Http.Get;
            try
            {
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                if (responseStream == null) throw new NoResponseFromServerException();
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                string responseString = readStream.ReadToEnd();
                List<Task> tasks = JsonConvert.DeserializeObject<List<Task>>(responseString);
                Console.WriteLine("tasks edited by this employee's subordinates:");
                foreach (Task task in tasks)
                {
                    Console.WriteLine($"{task.Id}, {task.Name}");
                }
                return tasks;
            }
            catch (WebException e)
            {
                Console.WriteLine("unable to get subordinate tasks");
                Console.Error.WriteLine(e.Message);
            }

            return null;
        }
        
        private static Report AddNewReport(Guid employeeId)
        {
            var request = WebRequest.Create($"https://localhost:5001/reports/addReport/?employeeId={employeeId}");
            request.Method = WebRequestMethods.Http.Post;
            try
            {
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                if (responseStream == null) throw new NoResponseFromServerException();
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                string responseString = readStream.ReadToEnd();
                Report report = JsonConvert.DeserializeObject<Report>(responseString);
                Console.WriteLine("Report successfully created");
                return report;
            }
            catch (WebException e)
            {
                Console.WriteLine("Unable to create a report");
                Console.Error.WriteLine(e.Message);
            }

            return null;
        }
        
        private static List<Report> GetAllReports()
        {
            var request = WebRequest.Create($"https://localhost:5001/reports/getReports/");
            request.Method = WebRequestMethods.Http.Get;
            try
            {
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                if (responseStream == null) throw new NoResponseFromServerException();
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                string responseString = readStream.ReadToEnd();
                List<Report> reports = JsonConvert.DeserializeObject<List<Report>>(responseString);
                Console.WriteLine("Reports:");
                foreach (var report in reports)
                {
                    Console.WriteLine($"id: {report.Id}, employee: {report.Employee}");
                }
                return reports;
            }
            catch (WebException e)
            {
                Console.WriteLine("unable to get reports");
                Console.Error.WriteLine(e.Message);
            }

            return null;
        }
        
        private static Report ChangeReportContent(Guid reportId, string message)
        {
            var request = WebRequest.Create($"https://localhost:5001/reports/changeContent/?reportId={reportId}&message={message}");
            request.Method = WebRequestMethods.Http.Post;
            try
            {
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                if (responseStream == null) throw new NoResponseFromServerException();
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                string responseString = readStream.ReadToEnd();
                Report report = JsonConvert.DeserializeObject<Report>(responseString);
                Console.WriteLine("Report's content successfully changed");
                
                return report;
            }
            catch (WebException e)
            {
                Console.WriteLine("unable to change report content");
                Console.Error.WriteLine(e.Message);
            }

            return null;
        }
        
        private static Report ChangeReportStatus(Guid reportId, ReportStatusType status)
        {
            var request = WebRequest.Create($"https://localhost:5001/reports/changeStatus/?reportId={reportId}&status={status}");
            request.Method = WebRequestMethods.Http.Post;
            try
            {
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                if (responseStream == null) throw new NoResponseFromServerException();
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                string responseString = readStream.ReadToEnd();
                Report report = JsonConvert.DeserializeObject<Report>(responseString);
                Console.WriteLine("Report's status successfully changed");
                
                return report;
            }
            catch (WebException e)
            {
                Console.WriteLine("unable to change report status");
                Console.Error.WriteLine(e.Message);
            }

            return null;
        }
        
        private static Report FinishReport(Guid reportId)
        {
            var request = WebRequest.Create($"https://localhost:5001/reports/finishReport/?reportId={reportId}");
            request.Method = WebRequestMethods.Http.Post;
            try
            {
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                if (responseStream == null) throw new NoResponseFromServerException();
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                string responseString = readStream.ReadToEnd();
                Report report = JsonConvert.DeserializeObject<Report>(responseString);
                Console.WriteLine("Report's status successfully changed");
                
                return report;
            }
            catch (WebException e)
            {
                Console.WriteLine("unable to change report status");
                Console.Error.WriteLine(e.Message);
            }

            return null;
        }
        
        private static List<Report> GetSubordinateReports(Guid employeeId)
        {
            var request = WebRequest.Create($"https://localhost:5001/reports/getSubordinateReports/?employeeId={employeeId}");
            request.Method = WebRequestMethods.Http.Get;
            try
            {
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                if (responseStream == null) throw new NoResponseFromServerException();
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                string responseString = readStream.ReadToEnd();
                List<Report> reports = JsonConvert.DeserializeObject<List<Report>>(responseString);
                Console.WriteLine("Subordinate reports:");
                foreach (var report in reports)
                {
                    Console.WriteLine($"id: {report.Id}, employee: {report.Employee}");
                }
                return reports;
            }
            catch (WebException e)
            {
                Console.WriteLine("unable to get subordinate reports");
                Console.Error.WriteLine(e.Message);
            }

            return null;
        }
        
        private static Report AddTaskToReport(Guid reportId, Guid taskId)
        {
            var request = WebRequest.Create($"https://localhost:5001/reports/addTaskToReport/?reportId={reportId}&taskId={taskId}");
            request.Method = WebRequestMethods.Http.Post;
            try
            {
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                if (responseStream == null) throw new NoResponseFromServerException();
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                string responseString = readStream.ReadToEnd();
                Report report = JsonConvert.DeserializeObject<Report>(responseString);
                Console.WriteLine("Task successfully added");
                
                return report;
            }
            catch (WebException e)
            {
                Console.WriteLine("unable to add task");
                Console.Error.WriteLine(e.Message);
            }

            return null;
        }
        
        private static WeeklyReport MakeWeeklyReport( Guid employeeId)
        {
            var request = WebRequest.Create($"https://localhost:5001/reports/makeWeeklyReport/?employeeId={employeeId}");
            request.Method = WebRequestMethods.Http.Post;
            try
            {
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                if (responseStream == null) throw new NoResponseFromServerException();
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                string responseString = readStream.ReadToEnd();
                WeeklyReport report = JsonConvert.DeserializeObject<WeeklyReport>(responseString);
                Console.WriteLine("Weekly report successfully made");
                Console.WriteLine($"id: {report.Id}");
                return report;
            }
            catch (WebException e)
            {
                Console.WriteLine("unable to create weekly report");
                Console.Error.WriteLine(e.Message);
            }

            return null;
        }
    
        private static List<WeeklyReport> GetWeeklyReports()
        {
            var request = WebRequest.Create($"https://localhost:5001/reports/getWeeklyReports/");
            request.Method = WebRequestMethods.Http.Get;
            try
            {
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                if (responseStream == null) throw new NoResponseFromServerException();
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                string responseString = readStream.ReadToEnd();
                List<WeeklyReport> reports = JsonConvert.DeserializeObject<List<WeeklyReport>>(responseString);
                Console.WriteLine("Weekly reports:");
                foreach (var report in reports)
                {
                    Console.WriteLine($"id: {report.Id}, employee: {report.Employee}");
                }
                return reports;
            }
            catch (WebException e)
            {
                Console.WriteLine("unable to get weekly reports");
                Console.Error.WriteLine(e.Message);
            }

            return null;
        }
        
        private static List<Task> GetWeeklyTasks(Guid weeklyReportId)
        {
            var request = WebRequest.Create($"https://localhost:5001/reports/getWeeklyTasks/?weeklyReportId={weeklyReportId}");
            request.Method = WebRequestMethods.Http.Get;
            try
            {
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                if (responseStream == null) throw new NoResponseFromServerException();
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                string responseString = readStream.ReadToEnd();
                List<Task> tasks = JsonConvert.DeserializeObject<List<Task>>(responseString);
                Console.WriteLine("Weekly tasks:");
                foreach (var task in tasks)
                {
                    Console.WriteLine($"id: {task.Id}");
                }
                return tasks;
            }
            catch (WebException e)
            {
                Console.WriteLine("unable to get tasks");
                Console.Error.WriteLine(e.Message);
            }

            return null;
        }
        
        private static List<Report> GetDailyReports(Guid weeklyReportId)
        {
            var request = WebRequest.Create($"https://localhost:5001/reports/getDailyReports/?weeklyReportId={weeklyReportId}");
            request.Method = WebRequestMethods.Http.Get;
            try
            {
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                if (responseStream == null) throw new NoResponseFromServerException();
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                string responseString = readStream.ReadToEnd();
                List<Report> reports = JsonConvert.DeserializeObject<List<Report>>(responseString);
                Console.WriteLine("Daily reports:");
                foreach (var report in reports)
                {
                    Console.WriteLine($"id: {report.Id}");
                }
                return reports;
            }
            catch (WebException e)
            {
                Console.WriteLine("unable to get daily reports");
                Console.Error.WriteLine(e.Message);
            }

            return null;
        }
        
        private static List<Employee> GetEmployeesWithReports(Guid weeklyReportId)
        {
            var request = WebRequest.Create($"https://localhost:5001/reports/getEmployeesWithReports/?weeklyReportId={weeklyReportId}");
            request.Method = WebRequestMethods.Http.Get;
            try
            {
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                if (responseStream == null) throw new NoResponseFromServerException();
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                string responseString = readStream.ReadToEnd();
                List<Employee> employees = JsonConvert.DeserializeObject<List<Employee>>(responseString);
                Console.WriteLine("Employees with reports:");
                foreach (var employee in employees)
                {
                    Console.WriteLine($"id: {employee.Id}");
                }
                return employees;
            }
            catch (WebException e)
            {
                Console.WriteLine("unable to get employees with reports");
                Console.Error.WriteLine(e.Message);
            }

            return null;
        }
        
        private static IEnumerable<Employee> GetEmployeesWithoutReports(Guid weeklyReportId)
        {
            var request = WebRequest.Create($"https://localhost:5001/reports/getEmployeesWithoutReports/?weeklyReportId={weeklyReportId}");
            request.Method = WebRequestMethods.Http.Get;
            try
            {
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                if (responseStream == null) throw new NoResponseFromServerException();
                using var readStream = new StreamReader(responseStream, Encoding.UTF8);
                string responseString = readStream.ReadToEnd();
                List<Employee> employees = JsonConvert.DeserializeObject<List<Employee>>(responseString);
                Console.WriteLine("Employees without reports:");
                foreach (var employee in employees)
                {
                    Console.WriteLine($"id: {employee.Id}");
                }
                return employees;
            }
            catch (WebException e)
            {
                Console.WriteLine("unable to get employees without reports");
                Console.Error.WriteLine(e.Message);
            }

            return null;
        }
    }
}
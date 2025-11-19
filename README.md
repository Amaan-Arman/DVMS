🏢 Digital Visitor Management System
A modern Digital Visitor Management System (DVMS) designed to streamline visitor registration, tracking, and management in offices, buildings, and institutions.
This system replaces traditional paper-based logs with a smart, secure, and efficient digital solution.

🚀 Features
🧾 Visitor Registration: Register invited and walk-in visitors digitally.
🕒 Check-In / Check-Out: Manage real-time visitor entry and exit logs.
👤 Host Notification: Notify hosts instantly when their visitor arrives.
🧑‍💼 Admin Dashboard: Monitor visitor activity, analytics, and reports.
📅 Visitor History: View and export visitor visit history with filters.
🔐 Secure Authentication: Role-based access for admin, host, and receptionist.
📸 Photo Capture: Capture visitor photos at entry for security purposes.
📊 Reports & Analytics: Generate daily, weekly, and monthly visitor summaries.
🧾 Generate printable visitor badges with QR codes.
🔔 Push notifications and email alerts.
📱 Mobile responsive interface.
🧰 Tech Stack
Layer	Technology Used
Frontend	HTML, CSS, JavaScript, Bootstrap
Backend	ASP.NET Core MVC
Database	Microsoft SQL Server
Real-Time Features	SignalR (for instant host notifications)
Authentication	Custom Authentication / Custom Role-Based Access Control
Version Control	Git & GitHub
⚙️ Installation
Follow these steps to set up the project locally:

Clone the repository

git clone https://github.com/your-username/Digital-Visitor-Management-System.git
Open the project Open the solution in Visual Studio.

Restore dependencies Right-click on the solution and select: Restore NuGet Packages

Set up the database Update the connection string in web.config or appsettings.json with your SQL Server credentials. Run the following commands in the Package Manager Console:

Run the project Press Ctrl + F5 or click Run in Visual Studio. The project will open in your browser.

👥 User Roles
Role	Description
Admin	Manage hosts, view reports, and overall system configuration
Host	View own visitors and receive arrival notifications
Receptionist	Register and manage visitor check-ins and check-outs
📂 Project Structure
Digital-Visitor-Management-System/
│ ├── Controllers/ # MVC controllers ├── Models/ # Entity models and view models ├── Views/ # Razor views (HTML templates) ├── Scripts/ # JS files ├── Content/ # CSS and static assets ├── wwwroot/ # Public assets (if .NET Core) ├── App_Data/ # Database or local data files ├── appsettings.json # Configuration file (.NET Core) └── web.config # Configuration file (.NET Framework)

📈 Future Enhancements
🧠 AI-based visitor analytics and reporting
🌍 Multi-location support
👨‍💻 Author
Developed by Amman Arman 📧 Email: amaanarman99@gmail.com 🔗 GitHub: https://github.com/Amaan-Arman

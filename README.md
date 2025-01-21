## ☁︎ Cloud Store ☁︎

A secure and efficient server application written in C# that lets users store, manage, and access files directly on their self-hosted servers. Take full control of your data with this simple yet powerful solution.
### 🌟 Features

  - Self-hosted: Your files, your server, your control.
  - User-friendly web interface: Easily upload, organize, and retrieve files from any device.
  - Cross-platform: Works seamlessly on Windows and Linux for hosting and on every device via browser
  - Open-source: Free to use and contribute to.

### 🛠️ Getting Started

Follow these steps to set up the application on your server.
Prerequisites

  - .NET 9.0 or higher installed on your server.
  - A server with appropriate firewall rules for HTTP/HTTPS access.

#### Installation

Clone this repository to your server:

    git clone https://github.com/ConnorMolz/Cloud-Store.git
    cd Cloud-Store

Run and build the application:

    docker compose up -d

Access the application in your browser at http://<your-server-ip>:<port> (default: http://localhost:8080).


### 🔒 Security Notes

  - Use a reverse proxy (e.g., NGINX, Apache) with SSL for production environments.
  - Regularly back up your storage path to prevent data loss.
  - Keep your .NET runtime and server OS updated for the latest security patches.

### 🤝 Contributing

We welcome contributions! To get started:

  - Fork this repository.
  - Create a new branch for your feature or fix:

        git checkout -b feature-name

  - Make your changes, commit, and push to your fork.
  - Open a pull request describing your changes.

### 📄 License

This project is licensed under the Apache License 2.0. See the LICENSE file for more information.
### 📢 Support

Have questions or feedback? Open an issue on GitHub or join the discussions.

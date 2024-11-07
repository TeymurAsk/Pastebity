# Pastebin api
This project is an API that replicates core functionality of [Link]Pastebin(https://pastebin.com/). It allows users to create, store, and share text blocks via unique links, with additional features for caching and data expiration management.
## Features
* **Text Storage and Sharing**: Users can create and retrieve text blocks by unique links.
* **Link and Password Hashing**: Uses secure hashing algorithms for generating links and storing password hashes.
* **Redis Caching**: Improves performance by caching frequently accessed data.
* **Data Expiration**: Implements a worker process that periodically removes expired data from the storage.

## Tech Stack
* **ASP.NET Web API**: The main framework for building the REST API.
* **PostgreSQL**: Relational database for storing text blocks and related metadata.
* **Redis**: Used for caching data to reduce database load and improve access times.
* **Amazon S3**: Stores text data to handle high traffic and scalable storage.
* **Docker**: Manages Redis instances locally or in production environments

## Getting Started
---

### Clone the repository

Clone the repo to get access to the code and provide in you IDE.

### Usage

1. You will need to run local instance of Redis on your machine. Try using Docker for local dev or some cloud services. **(The Application runs on localhost:20 port)**
2. You might need run migrations of the Database in your IDE after cloning the code.
3. Swagger Documentation provides all the info you need to use the API. Also, you can use for the login: admin@gmail.com and password: admin, to access Main endpoints.
4. You're ready to **GO**.

## Video Example





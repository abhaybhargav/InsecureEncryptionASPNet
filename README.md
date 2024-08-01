# InsecureEncryptionDemo

This project demonstrates secure and insecure encryption practices in an ASP.NET Core REST API application. It provides two modes of operation: secure and insecure, which can be toggled via configuration.

## Features

- User signup with email, password, and password hint
- User retrieval by ID
- Configurable encryption modes (secure and insecure)
- Docker support for easy deployment

## Getting Started

### Prerequisites

- Docker
- .NET 7.0 SDK (for local development)
- InsecureEncryptionDemo.csproj file in the project root directory

### Running the Application

1. Clone the repository:
   ```
   git clone https://github.com/yourusername/InsecureEncryptionDemo.git
   cd InsecureEncryptionDemo
   ```

2. Ensure the InsecureEncryptionDemo.csproj file exists in the project root directory.

3. Build the Docker image:
   ```
   docker build -t insecure-encryption-demo .
   ```

4. Run the Docker container:
   ```
   docker run -p 8880:8880 insecure-encryption-demo
   ```

The application will be accessible at `http://localhost:8880`.

## API Endpoints

### 1. User Signup

- **URL:** `/api/user/signup`
- **Method:** POST
- **Body:**
  ```json
  {
    "email": "user@example.com",
    "password": "password123",
    "passwordHint": "My favorite color"
  }
  ```
- **Response:**
  ```json
  {
    "id": "generated-guid",
    "email": "user@example.com",
    "message": "User created successfully"
  }
  ```

### 2. Get User

- **URL:** `/api/user/{userId}`
- **Method:** GET
- **Response:**
  ```json
  {
    "id": "user-guid",
    "email": "user@example.com",
    "encryptedPassword": "encrypted-password-string",
    "passwordHint": "My favorite color"
  }
  ```

## Encryption Modes

### Insecure Mode (Default)

In insecure mode, the application uses the following encryption settings:

- Algorithm: AES-128
- Mode: ECB (Electronic Codebook)
- Key: Static, hard-coded weak key ("insecurekey12345")
- Password Hint: Stored in plaintext

Vulnerabilities:
1. ECB mode is vulnerable to pattern analysis, as identical plaintext blocks produce identical ciphertext blocks.
2. The static, hard-coded key is easily compromised if the source code is exposed.
3. The short key length (128 bits) is more susceptible to brute-force attacks compared to longer key lengths.
4. Storing the password hint in plaintext can provide attackers with information to guess passwords.

### Secure Mode

In secure mode, the application uses the following encryption settings:

- Algorithm: AES-256
- Mode: CFB (Cipher Feedback)
- Key: Dynamically generated 256-bit key using a cryptographically secure random number generator
- Password Hint: Encrypted using the same secure method as the password

Improvements:
1. CFB mode provides better security by using the previous ciphertext block to encrypt the current plaintext block, preventing pattern analysis.
2. The dynamically generated key is unique for each application instance and not stored in the source code.
3. The longer key length (256 bits) provides stronger protection against brute-force attacks.
4. Encrypting the password hint prevents attackers from easily obtaining information to guess passwords.

## Switching Between Modes

To switch between secure and insecure modes:

1. Open the `appsettings.json` file.
2. Modify the `UseSecureEncryption` setting:
   ```json
   {
     "UseSecureEncryption": true
   }
   ```
   Set to `true` for secure mode, `false` for insecure mode.

3. Rebuild and run the Docker container:
   ```
   docker build -t insecure-encryption-demo .
   docker run -p 8880:8880 insecure-encryption-demo
   ```

## Security Considerations

This application is for demonstration purposes only and should not be used in a production environment. It intentionally includes insecure practices to highlight the differences between secure and insecure implementations.

In a real-world scenario:
- Always use secure encryption methods.
- Never store encryption keys in source code or version control.
- Use proper key management solutions for storing and rotating encryption keys.
- Implement additional security measures such as password hashing, salting, and rate limiting.
- Regularly update and patch all dependencies and the runtime environment.

## License

This project is licensed under the MIT License - see the LICENSE file for details.
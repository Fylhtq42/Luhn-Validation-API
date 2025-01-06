# Luhn Validation API

This API provides a single endpoint to check whether a given credit card number is **valid** according to
the [Luhn algorithm](https://en.wikipedia.org/wiki/Luhn_algorithm).

## Endpoint

### `POST /api/validate-credit-card`

**Description**  
Accepts a JSON payload containing a credit card number. Returns an indication of whether that number is valid (passes
Luhn checks) or invalid.

---

## Request Format

**Body**

```json
{
  "creditCardNumber": "string"
}
```

| Field              | Type   | Required | Description                         |
|--------------------|--------|----------|-------------------------------------|
| `creditCardNumber` | string | Yes      | The credit card number to validate. |

---

## Response Format

### **200 OK**

If the input was **processed** successfully, you'll receive a JSON object indicating whether the number is valid
according to the Luhn algorithm:

```json
{
  "isValid": true
}
```

or

```json
{
  "isValid": false
}
```

- `true` if the card number passes Luhn validation.
- `false` if it fails Luhn validation (depending on implementation—some implementations might throw an error instead).

### **400 Bad Request**

If the input is invalid (e.g., empty string, contains non-digit characters, etc.), the API returns:

```json
{
  "error": "Credit card number must contain only digits."
}
```

*(The exact message may vary. This indicates the server recognized invalid input.)*

### **500 Internal Server Error**

If an unexpected error occurs in the server, you receive:

```json
{
  "error": "An unexpected error occurred."
}
```

---

## Examples

### Valid Card Number Example

**Request**

```bash
curl -X POST \
  -H "Content-Type: application/json" \
  -d '{"creditCardNumber": "4532015112830366"}' \
  http://localhost:5000/api/validate-credit-card
```

**Successful Response (200)**

```json
{
  "isValid": true
}
```

---

### Luhn Fail Example

**Request**

```bash
curl -X POST \
  -H "Content-Type: application/json" \
  -d '{"creditCardNumber": "1234567890123456"}' \
  http://localhost:5000/api/validate-credit-card
```

**Response**  
Depending on your implementation:

- If failing Luhn **returns false**:
  ```json
  {
    "isValid": false
  }
  ```
- If failing Luhn **throws an exception**:
  ```json
  {
    "error": "Provided credit card number failed Luhn validation."
  }
  ```
  *(Returned with a `400 Bad Request` status.)*

---

### Non-Digit or Empty String

**Request**

```bash
curl -X POST \
  -H "Content-Type: application/json" \
  -d '{"creditCardNumber": "abc"}' \
  http://localhost:5000/api/validate-credit-card
```

**Response (400 Bad Request)**

```json
{
  "error": "Credit card number must contain only digits."
}
```

---

## Notes

- **Security**: Do not log or store credit card numbers in plain text in production environments.
- **Error Handling**: This API may return a 400 (Bad Request) for invalid inputs and a 500 (Internal Server Error) for
  unforeseen errors.
- **Luhn Algorithm**: [Read more about Luhn checks here](https://en.wikipedia.org/wiki/Luhn_algorithm).
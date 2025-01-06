# Luhn Validation API

This API provides a single endpoint to check whether a given credit card number is **valid** according to
the [Luhn algorithm](https://en.wikipedia.org/wiki/Luhn_algorithm). It also handles common formatting (e.g., spaces or
dashes) and enforces typical credit card length constraints (13–19 digits).

---

## Endpoint

### `POST /api/validate-credit-card`

Accepts a JSON payload containing a credit card number. Returns whether the number is valid according to the Luhn
algorithm, or an error if the input is invalid.

---

## Request Format

**Body**

```json
{
  "creditCardNumber": "string"
}
```

| Field              | Type   | Required | Description                                           |
|--------------------|--------|----------|-------------------------------------------------------|
| `creditCardNumber` | string | Yes      | The credit card number (with optional dashes/spaces). |

---

## Response Format

### 200 OK

If the input was valid and processed successfully, the response is:

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

- `true` if the (stripped) card number passes the Luhn check.
- `false` if it fails Luhn.

### 400 Bad Request

If the input is invalid (e.g., contains zero digits, is too short/long, or is empty), the API returns:

```json
{
  "error": "Some descriptive error message."
}
```

Exact messages vary depending on the specific error condition:

- **Empty or whitespace input**
- **No digits found** (e.g., `"----"` or all spaces)
- **Fewer than 13 digits or more than 19 digits**

### 500 Internal Server Error

If an unexpected error occurs on the server side, you receive:

```json
{
  "error": "An unexpected error occurred."
}
```

---

## Examples

### Valid Card Number (Strips Dashes)

**Request**

```bash
curl -X POST \
  -H "Content-Type: application/json" \
  -d '{"creditCardNumber": "4532-01511-2830-366"}' \
  http://localhost:5000/api/validate-credit-card
```

**Response (200)**

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

```json
{
  "isValid": false
}
```

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
  "error": "Credit card number cannot be empty or whitespace."
}
```

*(Exact wording may differ, e.g., `"No digits found"`, `"Must be between 13 and 19 digits."`, etc.)*

---

## Notes

- **Security**: Do not log or store credit card numbers in plain text. Consider masking or tokenizing if you must store
  them.
- **Error Handling**:
    - The API may return `400 Bad Request` if the input is empty, stripped of digits, or outside 13–19 digits.
    - A failing Luhn check results in `200` with `"isValid": false`.
- **Luhn Algorithm**:
    - [Learn more here](https://en.wikipedia.org/wiki/Luhn_algorithm).
    - The service strips common formatting (`-` or spaces) before running the Luhn check.
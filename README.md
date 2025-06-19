# ETL API for Yokomitsu-GreenPlus Integration

## Overview
A secure ETL (Extract-Transform-Load) API designed to automate retrieval of sales transaction and inventory data from GreenPlus software. Provides paginated endpoints for efficient large-scale data processing.

## Key Features
- **Sales Transaction API**  
  - Automated extraction from GreenPlus databases  
  - Paginated batch retrieval (`page` & `limit` parameters)  
  - Real-time data access  

- **Inventory API**  
  - Quantity tracking with optimized queries  
  - Chunked response handling  

- **Security**  
  - Token-based authentication  
  - Rate-limited endpoints  

## Technical Specs
| Component       | Details                          |
|-----------------|----------------------------------|
| Data Source     | GreenPlus software databases     |
| Response Format | JSON                             |
| Pagination      | `?page=1&limit=100`              |
| Auth            | JWT / API Keys                   |

## Usage Example
```bash
# Fetch sales transactions (100 records/page)
GET /api/sales/transactions?page=1&limit=100

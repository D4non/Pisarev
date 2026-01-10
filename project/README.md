# Clubs Management API

RESTful API для управления футбольными и баскетбольными клубами, игроками, тренерами и матчами. Построен на ASP.NET Core 8.0 с использованием PostgreSQL, Redis и Docker.


### Требования

- Docker и Docker Compose
- .NET 8 SDK (опционально, для локальной разработки)

### Запуск проекта

```bash
# Клонируйте репозиторий
git clone <repository-url>
cd dania

# Запустите все сервисы
docker-compose up --build

# Или в фоновом режиме
docker-compose up -d --build
```

### Swagger UI

После запуска проекта откройте Swagger UI для документации:

**http://localhost:5001/swagger**

Swagger предоставляет:
- Полный список всех endpoints
- Описание параметров запросов и ответов
- Возможность тестирования API прямо из браузера
- Схемы данных (DTO)

### Health Checks

Проверка состояния сервисов:

**http://localhost:5001/health**

Проверяет:
- Доступность PostgreSQL
- Доступность Redis
- Общее состояние API

### Prometheus Metrics

Метрики для мониторинга:

**http://localhost:5001/metrics**

## Аутентификация

API поддерживает два способа аутентификации:

### 1. JWT Bearer Token (для пользователей)

#### Регистрация нового пользователя

```http
POST /api/auth/register
Content-Type: application/json

{
  "username": "admin",
  "email": "admin@example.com",
  "password": "SecurePassword123!",
  "role": "Admin"
}
```

**Роли:**
- `Admin` - полный доступ ко всем операциям
- `Manager` - чтение, создание, обновление (без удаления)
- `User` - только чтение

#### Вход в систему

```http
POST /api/auth/login
Content-Type: application/json

{
  "username": "admin",
  "password": "SecurePassword123!"
}
```

**Ответ:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": 1,
    "username": "admin",
    "email": "admin@example.com",
    "role": "Admin"
  }
}
```

#### Использование токена

Добавьте токен в заголовок запроса:

```http
Authorization: Bearer {token}
```

### 2. API Key (для системных клиентов)

Передайте API ключ в заголовке:

```http
X-API-KEY: {your-api-key}
```

## API Endpoints

### Клубы (Clubs)

| Метод | Endpoint | Описание | Авторизация |
|-------|----------|----------|-------------|
| GET | `/api/clubs` | Получить список всех клубов | JWT/API Key |
| GET | `/api/clubs/{id}` | Получить клуб по ID | JWT/API Key |
| POST | `/api/clubs` | Создать новый клуб | Admin/Manager |
| PUT | `/api/clubs/{id}` | Обновить клуб | Admin/Manager |
| DELETE | `/api/clubs/{id}` | Удалить клуб | Admin |

**Пример создания клуба:**

```http
POST /api/clubs
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "Real Madrid",
  "type": 1,
  "city": "Madrid",
  "country": "Spain",
  "founded": "1902-03-06"
}
```

**Типы клубов:**
- `1` - Football (Футбол)
- `2` - Basketball (Баскетбол)

### Игроки (Players)

| Метод | Endpoint | Описание | Авторизация |
|-------|----------|----------|-------------|
| GET | `/api/players` | Получить список всех игроков | JWT/API Key |
| GET | `/api/players/{id}` | Получить игрока по ID | JWT/API Key |
| POST | `/api/players` | Создать нового игрока | Admin/Manager |
| PUT | `/api/players/{id}` | Обновить игрока | Admin/Manager |
| DELETE | `/api/players/{id}` | Удалить игрока | Admin |

**Пример создания игрока:**

```http
POST /api/players
Authorization: Bearer {token}
Content-Type: application/json

{
  "firstName": "Cristiano",
  "lastName": "Ronaldo",
  "dateOfBirth": "1985-02-05",
  "position": "Forward",
  "jerseyNumber": 7
}
```

### Тренеры (Coaches)

| Метод | Endpoint | Описание | Авторизация |
|-------|----------|----------|-------------|
| GET | `/api/coaches` | Получить список всех тренеров | JWT/API Key |
| GET | `/api/coaches/{id}` | Получить тренера по ID | JWT/API Key |
| POST | `/api/coaches` | Создать нового тренера | Admin/Manager |
| PUT | `/api/coaches/{id}` | Обновить тренера | Admin/Manager |
| DELETE | `/api/coaches/{id}` | Удалить тренера | Admin |

### Матчи (Matches)

| Метод | Endpoint | Описание | Авторизация |
|-------|----------|----------|-------------|
| GET | `/api/matches` | Получить список всех матчей | JWT/API Key |
| GET | `/api/matches/{id}` | Получить матч по ID | JWT/API Key |
| POST | `/api/matches` | Создать новый матч | Admin/Manager |
| PUT | `/api/matches/{id}` | Обновить матч | Admin/Manager |
| DELETE | `/api/matches/{id}` | Удалить матч | Admin |

##  Архитектура проекта

Проект следует принципу разделения ответственности и состоит из следующих слоев:

```
Controllers (API Layer)
    ↓
Services (Business Logic)
    ↓
Repositories (Data Access)
    ↓
Database (PostgreSQL)
```

### Структура проекта

```
project/
├── Controllers/          # API контроллеры
│   ├── AuthController.cs
│   ├── ClubsController.cs
│   ├── PlayersController.cs
│   ├── CoachesController.cs
│   └── MatchesController.cs
│
├── Services/            # Бизнес-логика
│   ├── Interfaces/     # Интерфейсы сервисов
│   ├── AuthService.cs
│   ├── ClubService.cs
│   ├── PlayerService.cs
│   ├── CoachService.cs
│   └── MatchService.cs
│
├── Repositories/        # Доступ к данным
│   ├── Interfaces/     # Интерфейсы репозиториев
│   ├── ClubRepository.cs
│   ├── PlayerRepository.cs
│   ├── CoachRepository.cs
│   ├── MatchRepository.cs
│   ├── UserRepository.cs
│   └── ApiKeyRepository.cs
│
├── Models/             # Модели данных
│   ├── Entities/       # Доменные модели
│   └── DTO/           # Data Transfer Objects
│
├── Data/               # DbContext
│   └── ApplicationDbContext.cs
│
├── Middleware/         # Промежуточное ПО
│   ├── ErrorHandlingMiddleware.cs
│   ├── RateLimitingMiddleware.cs
│   ├── IdempotencyMiddleware.cs
│   ├── RequestLoggingMiddleware.cs
│   └── ApiKeyAuthenticationHandler.cs
│
├── Validators/         # FluentValidation валидаторы
│
└── Liquibase/         # Миграции БД
    ├── master.xml
    └── change-sets/
```

##  Технологический стек

### Backend
- **ASP.NET Core 8.0** - Web framework
- **Entity Framework Core** - ORM для работы с БД
- **Npgsql** - PostgreSQL провайдер
- **FluentValidation** - Валидация входных данных
- **JWT Bearer** - Аутентификация через токены
- **BCrypt** - Хеширование паролей

### База данных
- **PostgreSQL 16** - Реляционная БД
- **Liquibase** - Миграции БД

### Кэширование
- **Redis 7** - Распределенный кэш
  - Кэширование данных
  - Rate limiting счетчики
  - Идемпотентность ключи

### Логирование и мониторинг
- **Serilog** - Структурированное логирование
- **Prometheus** - Метрики приложения
- **Health Checks** - Проверка здоровья сервисов

### Документация
- **Swagger/OpenAPI** - Документация API

### Инфраструктура
- **Docker** - Контейнеризация
- **Docker Compose** - Оркестрация контейнеров

## Конфигурация

### Переменные окружения

Основные настройки находятся в `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=db;Port=5432;Database=test_db;Username=test_db;Password=test_db",
    "Redis": "redis:6379"
  },
  "JwtSettings": {
    "SecretKey": "YourSuperSecretKeyThatShouldBeAtLeast32CharactersLong!",
    "Issuer": "ClubsManagementAPI",
    "Audience": "ClubsManagementAPI",
    "ExpirationMinutes": 60
  }
}
```

В Docker Compose переменные окружения переопределяются через environment variables.

### Порты

- **API**: `5001:80` (внешний:внутренний)
- **PostgreSQL**: `5439:5432`
- **Redis**: `6380:6379`


### Кэширование

- GET запросы к клубам кэшируются в Redis на 5 минут
- Кэш автоматически инвалидируется при обновлении или удалении
- При ошибке десериализации кэш очищается

### Обработка ошибок

Все ошибки обрабатываются глобальным middleware и возвращаются в стандартизированном формате:

```json
{
  "error": "InternalServerError",
  "message": "An error occurred while processing your request.",
  "traceId": "0HNIBSB91OF5T:00000001"
}
```

### Логирование

Все запросы логируются через Serilog с информацией о:
- Методе и пути запроса
- Времени выполнения
- Статус коде ответа
- Размере запроса/ответа

## База данных

### Схема данных

Основные сущности:
- **clubs** - Клубы
- **players** - Игроки
- **coaches** - Тренеры
- **matches** - Матчи
- **stadiums** - Стадионы
- **users** - Пользователи
- **api_keys** - API ключи

### Связи

- **Club ↔ Player** (many-to-many через `club_players`)
- **Club ↔ Coach** (many-to-many через `club_coaches`)
- **Club ↔ Stadium** (one-to-one)
- **Club ↔ Match** (one-to-many: HomeClub, AwayClub)
- **Player ↔ Match** (many-to-many через `player_matches`)

### Миграции

Миграции выполняются автоматически через Liquibase при запуске контейнера `migrations`.

Файлы миграций:
- `001_initial_schema.sql` - Создание всех таблиц
- `002_seed_data.sql` - Начальные данные

## Тестирование

### Через Swagger

1. Откройте http://localhost:5001/swagger
2. Нажмите "Authorize" и введите JWT токен
3. Выберите endpoint и нажмите "Try it out"
4. Заполните параметры и нажмите "Execute"

# Вход
curl -X POST http://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "username": "testuser",
    "password": "Password123!"
  }'

# Получение списка клубов
curl -X GET http://localhost:5001/api/clubs \
  -H "Authorization: Bearer {token}"
```

## Мониторинг

### Health Checks

```bash
curl http://localhost:5001/health
```

### Prometheus Metrics

```bash
curl http://localhost:5001/metrics
```
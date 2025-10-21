OrdersMini API
 Описание проекта
Этот проект представляет собой API для управления заказами, использующий PostgreSQL для хранения данных о заказах и MongoDB для хранения истории изменений заказов (создание, обновление, удаление). Проект построен с использованием .NET 8 (или 6) и следует принципам Clean Architecture.

API взаимодействует с базой данных PostgreSQL для хранения данных о заказах и записывает действия с заказами в MongoDB для аудита.

 Инструкция по запуску
1. Необходимые инструменты
Перед тем как начать, убедитесь, что у вас установлены следующие инструменты:

Docker: для запуска контейнеров с базами данных.

.NET SDK: для запуска API проекта.

2. Запуск контейнеров PostgreSQL и MongoDB с Docker Compose
Для запуска контейнеров с PostgreSQL и MongoDB используйте команду:

Bash

docker-compose up -d
Эта команда:

Запускает PostgreSQL на localhost:55432 (порт 5432 внутри контейнера).

Запускает MongoDB на localhost:27018 (порт 27017 внутри контейнера).

Проверить, что контейнеры запущены, можно командой:

Bash

docker ps
3. Применение миграций для PostgreSQL
После того как контейнеры запущены, необходимо применить EF Core миграции, чтобы создать таблицы в базе данных PostgreSQL.

Откройте Package Manager Console в Visual Studio и выполните команду:

PowerShell

Update-Database -StartupProject Orders.API -Project Orders.Infrastructure
Эта команда создаст таблицы в базе данных PostgreSQL и выполнит все миграции, определённые в проекте.

4. Запуск API
Для запуска API локально используйте следующую команду:

Bash

dotnet run --project Orders.API
📡 API Эндпоинты
1. POST /api/orders — создание нового заказа
Тело запроса:

JSON

{
  "customerName": "Алиса",
  "totalAmount": 100.00,
  "status": "New"
}
Ответ:

JSON

{
  "id": "some-uuid"
}
Этот запрос создаёт новый заказ с указанными полями. Ответ содержит id созданного заказа.

2. GET /api/orders — получение всех заказов
Ответ:

JSON

[
  {
    "id": "some-uuid",
    "customerName": "Алиса",
    "totalAmount": 100.00,
    "status": "New",
    "createdAt": "2025-10-20T11:22:45.382269+00:00"
  }
]
Возвращает список всех заказов в системе.

3. GET /api/orders/{id} — получение конкретного заказа по ID
Параметры:

id — уникальный идентификатор заказа.

Ответ:

JSON

{
  "id": "some-uuid",
  "customerName": "Алиса",
  "totalAmount": 100.00,
  "status": "New",
  "createdAt": "2025-10-20T11:22:45.382269+00:00"
}
Возвращает детальную информацию о заказе по указанному id.

4. PUT /api/orders/{id} — обновление заказа
Тело запроса:

JSON

{
  "id": "some-uuid",
  "customerName": "Боб",
  "totalAmount": 150.00,
  "status": "Shipped"
}
Ответ: 204 No Content — если заказ успешно обновлён.

5. DELETE /api/orders/{id} — удаление заказа
Ответ: 204 No Content — если заказ успешно удалён.

 Использование MongoDB
Коллекция OrderHistory
Коллекция OrderHistory в MongoDB хранит историю изменений заказов. Каждая запись содержит:

Action — тип действия: Created, Updated, Deleted

Old Value — состояние заказа до изменения (если было обновление)

New Value — новое состояние заказа

Timestamp — дата и время изменения

Получить доступ к коллекции OrderHistory можно через MongoDB Compass или консоль mongosh.

Пример записи в MongoDB:

JSON

{
  "orderId": "f51067cb-faaf-472f-9669-11702caa57da",
  "action": "Updated",
  "timestamp": "2025-10-20T12:00:00.000Z",
  "oldValue": {
    "customerName": "Жанна Смит",
    "totalAmount": 199.99,
    "status": "New"
  },
  "newValue": {
    "customerName": "Жанна Смит",
    "totalAmount": 199.99,
    "status": "Shipped"
  }
}
 Устранение неполадок
PostgreSQL и MongoDB не запускаются Если контейнеры не запущены, попробуйте перезапустить их с помощью:

Bash

docker-compose down
docker-compose up -d
Не удаётся подключиться к MongoDB Убедитесь, что MongoDB запущен на localhost:27018 и правильно настроен в docker-compose.yml.

Миграции базы данных не применены Если миграции не были применены, выполните следующую команду:

PowerShell

Update-Database -StartupProject Orders.API -Project Orders.Infrastructure
 Заключение
 Тестирование API
Используйте Swagger: http://localhost:5000/swagger

Также можно использовать Postman для отправки запросов вручную.

 Сохранение данных
Данные в базах PostgreSQL и MongoDB сохраняются в Docker volume, поэтому информация не теряется даже после остановки или перезапуска контейнеров.
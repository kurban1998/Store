# Store

Сайт для продажи футболок
1. WebApp - сам сайт, клиентская часть проекта.

2. TshirtStoreApi - API проекта к которому обращается WebApp.
   TshirtStoreApi обращается к БД, для получения всех футболок.
   Также имеет BackgroundService, который обновляет кеш через
   определенное время.

3. CustomerManagementService - отвечает за пользователей сервиса. 

4. ServiceLoyalityManagenet - отвечает за покупателей, которые  часто покупают товар и начисляет им бонусы.

5. TshirtManagementService - выполняет функции CRUD.

6. БД
    а) Ms sql server, имеет три таблицы связанные между собой Tshirts (футболки), Reviews(отзывы), PriceOffers(Скидки) и отдельная таблица LoyalCustomers(постоянные покупатели, которым начисляют бонусы). 
    б) Redis - хранит данные о футболках для того, чтобы разгрузить систему и каждый раз не обращаться к БД.

Технологии
1. RabbitMq - основной брокер сообщений. Использовал пакет MassTransitMq.

Паттерны
1. Builder - используется для создания основной сущности Tshirt

Примитивы синхронизации
1. Использован синт. сахар lock (Monitor) для обновления и начисления баллов, постоянным покупателям.

Обновлено: 31.08.2024

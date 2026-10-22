package com.ecommerce.notification.listener;

import com.ecommerce.notification.dto.OrderPlacedEvent;
import com.ecommerce.notification.service.EmailService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.kafka.annotation.KafkaListener;
import org.springframework.stereotype.Component;

@Component
public class OrderEventListener {

    private static final Logger log = LoggerFactory.getLogger(OrderEventListener.class);
    private final EmailService emailService;

    public OrderEventListener(EmailService emailService) {
        this.emailService = emailService;
    }

    @KafkaListener(
            topics = "order-placed-topic",
            groupId = "notification-group",
            containerFactory = "kafkaListenerContainerFactory"
    )
    public void handleOrderPlacedEvent(OrderPlacedEvent event) {
        log.info("[Kafka Notification Consumer] Event intercepted from 'order-placed-topic' for Order: {}", event.getOrderNumber());
        emailService.sendOrderConfirmationEmail(event);
    }
}

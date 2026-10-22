package com.ecommerce.notification.service;

import com.ecommerce.notification.dto.OrderPlacedEvent;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.mail.SimpleMailMessage;
import org.springframework.mail.javamail.JavaMailSender;
import org.springframework.stereotype.Service;

@Service
public class EmailService {

    private static final Logger log = LoggerFactory.getLogger(EmailService.class);

    @Autowired(required = false)
    private JavaMailSender mailSender;

    public void sendOrderConfirmationEmail(OrderPlacedEvent event) {
        log.info("--------------------------------------------------------------------------------");
        log.info("[NOTIFICATION-SERVICE] Processing order confirmation");
        log.info("Order ID       : #{}", event.getOrderId());
        log.info("Order Number   : {}", event.getOrderNumber());
        log.info("Customer ID    : {}", event.getCustomerId());
        log.info("Customer Email : {}", event.getCustomerEmail() != null ? event.getCustomerEmail() : "(Customer ID " + event.getCustomerId() + ")");
        log.info("Total Amount   : ${}", event.getTotalAmount());
        log.info("Items Count    : {}", event.getItems() != null ? event.getItems().size() : 0);

        if (event.getItems() != null && !event.getItems().isEmpty()) {
            event.getItems().forEach(item ->
                log.info("   -> ProductVariantId: {} | Quantity: {}", item.getProductVariantId(), item.getQuantity())
            );
        }
        log.info("--------------------------------------------------------------------------------");

        try {
            if (mailSender != null && event.getCustomerEmail() != null && !event.getCustomerEmail().isBlank()) {
                SimpleMailMessage message = new SimpleMailMessage();
                message.setTo(event.getCustomerEmail());
                message.setSubject("Order Confirmation - " + event.getOrderNumber());
                message.setText(
                    "Hello Customer #" + event.getCustomerId() + ",\n\n" +
                    "Thank you for shopping with us!\n" +
                    "Your order " + event.getOrderNumber() + " has been placed successfully.\n" +
                    "Total Amount: $" + event.getTotalAmount() + "\n\n" +
                    "We are preparing your items for delivery.\n\n" +
                    "Best regards,\nE-Commerce Team"
                );
                mailSender.send(message);
                log.info("[SUCCESS] Live email successfully dispatched to {}", event.getCustomerEmail());
            } else {
                log.info("[SIMULATED NOTIFICATION] Email sent successfully to Customer #{} via simulated mail pipeline.", event.getCustomerId());
            }
        } catch (Exception ex) {
            log.warn("[WARNING] Could not send live email (SMTP might not be configured): {}. Simulated notification logged.", ex.getMessage());
        }
    }
}

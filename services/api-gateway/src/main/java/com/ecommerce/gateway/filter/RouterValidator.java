package com.ecommerce.gateway.filter;

import org.springframework.http.HttpMethod;
import org.springframework.http.server.reactive.ServerHttpRequest;
import org.springframework.stereotype.Component;

import java.util.List;
import java.util.function.Predicate;

@Component
public class RouterValidator {

    public static final List<String> OPEN_API_ENDPOINTS = List.of(
            "/api/auth/register",
            "/api/auth/login",
            "/api/auth/refresh",
            "/swagger-ui",
            "/v3/api-docs"
    );

    public Predicate<ServerHttpRequest> isSecured = request -> {
        String path = request.getURI().getPath();

        if (OPEN_API_ENDPOINTS.stream().anyMatch(path::startsWith)) {
            return false;
        }

        if (request.getMethod() == HttpMethod.GET &&
                (path.startsWith("/api/Products") || path.startsWith("/api/Categories") || path.startsWith("/api/Brands"))) {
            return false;
        }

        return true;
    };
}

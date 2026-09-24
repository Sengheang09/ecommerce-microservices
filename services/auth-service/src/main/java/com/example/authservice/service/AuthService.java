package com.example.authservice.service;

import com.example.authservice.dto.Request.LoginRequest;
import com.example.authservice.dto.Request.RefreshTokenRequest;
import com.example.authservice.dto.Request.RegisterRequest;
import com.example.authservice.dto.Response.AuthResponse;
import com.example.authservice.dto.Response.UserResponse;

public interface AuthService {

    UserResponse register(RegisterRequest request);

    AuthResponse login(LoginRequest request);

    AuthResponse refreshToken(RefreshTokenRequest request);

    void logout(String refreshToken);

    UserResponse getCurrentUser(String email);

}

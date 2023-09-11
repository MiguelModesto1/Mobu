import React,{ useState } from "react";
import Input from "../../modular/Input";
import ClickableIcon from "../../modular/ClickableIcon";
import "./MessageFooterBar.css";

export default function MessageFooterBar(){

    /* IMPLEMENTAR COM ESTADOS */
    
    return(
        <div className="message-header-bar">
            <ClickableIcon 
            CIProps={{
                size:"66px",
                fill:"none",
                path:{
                    d:"M16 28C16 28 19 32 24 32C29 32 32 28 32 28M30 18H30.02M18 18H18.02M44 24C44 35.0457 35.0457 44 24 44C12.9543 44 4 35.0457 4 24C4 12.9543 12.9543 4 24 4C35.0457 4 44 12.9543 44 24ZM31 18C31 18.5523 30.5523 19 30 19C29.4477 19 29 18.5523 29 18C29 17.4477 29.4477 17 30 17C30.5523 17 31 17.4477 31 18ZM19 18C19 18.5523 18.5523 19 18 19C17.4477 19 17 18.5523 17 18C17 17.4477 17.4477 17 18 17C18.5523 17 19 17.4477 19 18Z",
                    stroke:"#6c6c6c",
                    strokeWidth:"2",
                    strokeLineCap:"round",
                    strokeLinejoin:"round"
                }
            }}
            onClick={() => {alert("Implementar onclick de MessageFooterBar emojis icon");}}
            fromParent="footer-bar" />
            <div className="footer-bar-input-div">
                <Input input={{
                type:"text",
                tile:"",
                value:"",
                placeholder:""
                }}
                fromParent="footer-bar"/>
            </div>
            
            <ClickableIcon 
            CIProps={{
                size:"71px",
                fill:"none",
                path:{
                    d:"M24.0009 37.9999V23.9999M24.5839 38.1692L38.5409 42.8416C39.6347 43.2078 40.1816 43.3909 40.5188 43.2595C40.8117 43.1454 41.0339 42.9002 41.1187 42.5976C41.2164 42.2491 40.9806 41.7228 40.5089 40.6701L25.532 7.24465C25.0708 6.21525 24.8402 5.70055 24.5189 5.54106C24.2398 5.4025 23.912 5.40198 23.6325 5.53964C23.3107 5.69809 23.0784 6.21205 22.6139 7.23996L7.50487 40.672C7.02948 41.724 6.79179 42.2499 6.88845 42.5988C6.97239 42.9018 7.19393 43.1477 7.48657 43.2626C7.82355 43.395 8.37134 43.2131 9.46691 42.8495L23.572 38.1679C23.7598 38.1056 23.8537 38.0744 23.9497 38.0621C24.0349 38.0512 24.1212 38.0513 24.2064 38.0624C24.3024 38.075 24.3963 38.1064 24.5839 38.1692Z",
                    stroke:"#6c6c6c",
                    strokeWidth:"2",
                    strokeLineCap:"round",
                    strokeLinejoin:"round"
                }
            }}
            onClick={() => {alert("Implementar onclick de MessageFooterBar send icon");}}
            fromParent="footer-bar" />
        </div>
    );

    
}
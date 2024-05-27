import React, { useRef, useEffect, useState, useMemo, useLayoutEffect, useCallback } from "react";

/**
 * 
 * Propriedade de perfil
 * 
 */
export default function ProfileProperty({ keyProp, isEditing, isAvatar = null, isBirthDate = null, isText = null, text, onChangeText = null }) {

    const firstTextValue = useRef(text);

    const [hasChangedAvatar, setHasChangedAvatar] = useState(false);
    const [valueType, setValueType] = useState("");
    const [inputValue, setInputValue] = useState(firstTextValue.current);

    useEffect(() => {
        if (isEditing) {

            if (isAvatar && isAvatar !== null) {
                setValueType("file");
            }
            else if (isText && isText !== null) {
                setValueType("text");
            }
            else if (isBirthDate && isBirthDate !== null) {
                setValueType("date");
            }
            else {
                setValueType("password")
            }
        }
    }, []);

    useEffect(() => {
        
        if (isAvatar) {
            document.getElementsByClassName('avatar')[0].setAttribute('src', hasChangedAvatar ? text
                : "../../../assets/images/default_avatar.png");
        }

    }, [isAvatar]);

    /**
     * callback para a mudanca do avatar
     */
    const handleAvatarChange = () => {
        setHasChangedAvatar(true)
    }

    const handleInputValueChange = (value) => {
        setInputValue(value)
    }

    return (
        <div className="profile-prop-div">
            <span className="profile-key-span">{keyProp + " : "}</span>

            {!isEditing ?
                <span className="profile-value-span">{inputValue}</span>
                :
                <input
                    type={valueType}
                    value={!isAvatar ? inputValue : undefined}
                    className="profile-input"
                    accept={isAvatar ? ".jpg,.jpeg,.png" : undefined}
                    onChange={e => {
                        if (isAvatar && isAvatar !== null) {
                            onChangeText(e.target);
                            if (!hasChangedAvatar) {
                                handleAvatarChange();
                            }
                        }
                        else {
                            onChangeText(e.target.value);
                            handleInputValueChange(e.target.value);
                        } 
                    }}
                />
            }
        </div>
    );

}
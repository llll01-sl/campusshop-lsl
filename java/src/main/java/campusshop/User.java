package campusshop;

/** 用户。 */
public record User(String username, String password, String address, String phone, MemberLevel level) {

    /** 普通会员。 */
    public User(String username, String password, String address, String phone) {
        this(username, password, address, phone, MemberLevel.NORMAL);
    }
}

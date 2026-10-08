package campusshop;

/** 会员等级。 */
public enum MemberLevel {
    /** 普通会员：不打折。 */
    NORMAL("普通"),
    /** 银卡会员：9.5 折。 */
    SILVER("银卡"),
    /** 金卡会员：9 折。 */
    GOLD("金卡");

    private final String chineseName;

    MemberLevel(String chineseName) {
        this.chineseName = chineseName;
    }

    public String getChineseName() {
        return chineseName;
    }

    /** 按中文名称（普通、银卡、金卡）查找会员等级。 */
    public static MemberLevel fromChineseName(String name) {
        for (MemberLevel level : values()) {
            if (level.chineseName.equals(name))
                return level;
        }
        throw new IllegalArgumentException("未知的会员等级：" + name);
    }
}

package eu.nvna.classes;

public class Rectangle {
    private int iX1;
    private int iY1;
    private int iX2;
    private int iY2;

    public Rectangle() {
    }

    public Rectangle(int x1, int y1, int x2, int y2) {
        iX1 = Math.min(x1, x2);
        iY1 = Math.min(y1, y2);
        iX2 = Math.max(x1, x2);
        iY2 = Math.max(y1, y2);
    }

    public int getIX1() {
        return iX1;
    }

    public void setIX1(int x1) {
        if (x1 <= iX2) {
            iX1 = x1;
        } else {
            iX1 = iX2;
            iX2 = x1;
        }
    }

    public int getIX2() {
        return iX2;
    }

    public void setIX2(int x2) {
        if (x2 >= iX1) {
            iX2 = x2;
        } else {
            iX2 = iX1;
            iX1 = x2;
        }
    }

    public int getIY1() {
        return iY1;
    }

    public void setIY1(int y1) {
        if (y1 <= iY2) {
            iY1 = y1;
        } else {
            iY1 = iY2;
            iY2 = y1;
        }
    }

    public int getIY2() {
        return iY2;
    }

    public void setIY2(int y2) {
        if (y2 >= iY1) {
            iY2 = y2;
        } else {
            iY2 = iY1;
            iY1 = y2;
        }
    }

    public int Area() {
        return Math.abs((iX2 - iX1) * (iY2 - iY1));
    }

    public int compareTo(Object r) {
        return Integer.compare(this.Area(), ((Rectangle) r).Area());
    }

    public String toString() {
        return "iX1: " + iX1 + " iY1: " + iY1 + " | iX2: " + iX2 + " iY2: " + iY2;
    }


    public boolean equals(Rectangle r) {
        return this.Area() == r.Area();
    }

    public void translateX(int iPoints) {
        iX1 += iPoints;
        iX2 += iPoints;
    }

    public void translateY(int iPoints) {
        iY1 += iPoints;
        iY2 += iPoints;
    }

    public boolean isInside(int x, int y) {
        return ((x > iX1) && (x < iX2) && (y > iY1) && (y < iY2));
    }


    public Rectangle unionRect(Rectangle r) {
        return new Rectangle(Math.min(this.iX1, r.iX1),
                Math.min(this.iY1, r.iY1),
                Math.max(this.iX2, r.iX2),
                Math.max(this.iY2, r.iY2));
    }

    public Rectangle intersectionRect(Rectangle r) {
        Rectangle result = new Rectangle(Math.max(this.iX1, r.iX1),
                Math.max(this.iY1, r.iY1),
                Math.min(this.iX2, r.iX2),
                Math.min(this.iY2, r.iY2));
        if (result.iX1 > result.iX2) {
            result.iX1 = result.iX2 = 0;
        }
        if (result.iY1 > result.iY2) {
            result.iY1 = result.iY2 = 0;
        }
        return result;
    }

    /**
     * @param args
     */
    public static void main(String[] args) {
        Rectangle oRect = new Rectangle(0, 0, 10, 10);
        System.out.println(oRect.toString());
        oRect.translateX(10);
        System.out.println(oRect.toString());
        oRect.translateX(-20);
        System.out.println(oRect.toString());
        oRect.translateY(10);
        System.out.println(oRect.toString());
        Rectangle oRect2 = new Rectangle(0, 0, 10, 10);
        System.out.println(oRect2.toString());
        if (oRect.equals(oRect2)) {
            System.out.println("equal OK");
        } else {
            System.out.println("equal FALSE");
        }
        Rectangle resUnion = oRect.unionRect(oRect2);
        System.out.println("Union:" + resUnion.toString() + " Area:" + resUnion.Area());
        Rectangle resInters = oRect.intersectionRect(oRect2);
        System.out.println("Inters:" + resInters.toString() + " Area:" + resInters.Area());

    }
}